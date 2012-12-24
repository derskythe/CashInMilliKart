using System;
using System.Threading;
using CashControlTerminal;

namespace CashInTerminal
{
    public sealed class CcnetDevice : IDisposable
    {
        #region Fields

        static private SerialStream _SerialDevice;

        static private bool _StartPool;
        static private bool _ParseRead;

        private const int POLLING_INTERVAL = 1000;
        private const int MAX_RESPONSE_TIME = 250;
        private const int MAX_PACKET_LENGTH = 255;

        private readonly Thread _ResponseReaderThread;
        private readonly Thread _PoolThread;
        private readonly Thread _SendThread;
        private Thread _EventThread;
        private readonly CcnetDeviceState _DeviceState;
        private readonly string _Currency; // Название валюты        
        private int _Port;
        private CcnetPortSpeed _PortSpeed;
        private readonly Object _ResponseSignal;
        private CCNETPacket _ResponsePacket;
        private readonly EventWaitHandle _Wait = new AutoResetEvent(false);
        private bool _TimedOut;
        private bool _Disposing = false;

        public delegate void ReadCommandDelegate(CcnetDeviceState e);
        public delegate void BillStackedDelegate(CcnetDeviceState e);
        public event ReadCommandDelegate ReadCommand;
        public event BillStackedDelegate BillStacked;

        #endregion

        #region Properties

        public bool StartPool
        {
            get
            {
                return _StartPool;
            }
            set
            {
                _StartPool = value;

                if (_StartPool == true)
                {
                    _Wait.Set();
                }
            }
        }

        public CcnetDeviceState DeviceState
        {
            get
            {
                return _DeviceState;
            }
        }

        public string Currency
        {
            get
            {
                return _Currency;
            }
        }

        #endregion

        #region Constructor/Destructor/Dispose

        public CcnetDevice(Thread sendThread)
        {
            _SendThread = sendThread;
            _StartPool = false;
            _DeviceState = new CcnetDeviceState();
            //if (currency.Length == 0)
            //{
            //    _Currency = "azn";
            //}
            //else
            //{
            //    _Currency = currency;
            //}
            _Currency = "azn";
            _ResponseSignal = new Object();

            // Create the Thread used to read responses
            _ResponseReaderThread = new Thread(ReadResponseThread);
            _ResponseReaderThread.IsBackground = true;

            // Create the Thread used to send POOL command
            _PoolThread = new Thread(StartPooling) {IsBackground = true};

            _SerialDevice = new SerialStream();

        }

        public void Open(int port, CcnetPortSpeed speed)
        {
            try
            {
                _Port = port;
                _PortSpeed = speed;

                // Make sure to use the right string,
                // this is directly parsed to the CreateFile
                // "\\\\.\\COM3",
                _SerialDevice.Open("\\\\.\\COM" + _Port);
            }
            catch (Exception e)
            {
                // "Error: " + e.Message, 
                return;
            }

            _SerialDevice.SetPortSettings(
                (uint)_PortSpeed,
                SerialStream.FlowControl.None,
                SerialStream.Parity.None,
                8,
                SerialStream.StopBits.One);

            // Set timeout so read ends after 20ms of silence after a response
            _SerialDevice.SetTimeouts(200, 11, 200, 11, 200);

            if (_ResponseReaderThread.ThreadState == (ThreadState.Background | ThreadState.Unstarted))
            {
                _ResponseReaderThread.Start();
            }

            if (_PoolThread.ThreadState == (ThreadState.Background | ThreadState.Unstarted))
            {
                _PoolThread.Start();
            }
            _ParseRead = true;
        }

        public void Close()
        {
            if (!_SerialDevice.Closed)
            {
                _SerialDevice.Close();
            }

            if (_ResponseReaderThread.ThreadState == ThreadState.Running)
            {
                _ParseRead = false;
            }

            if (_PoolThread.ThreadState == ThreadState.Running)
            {
                _StartPool = false;
            }
        }

        public void Dispose()
        {
            _Disposing = true;
            if (_ResponseReaderThread != null && _ResponseReaderThread.IsAlive)
            {
                _ResponseReaderThread.Join(5000);
            }

            if (_PoolThread != null && _PoolThread.IsAlive)
            {
                _PoolThread.Join(5000);
            }

            if (_SendThread != null && _SendThread.IsAlive)
            {
                _SendThread.Join(5000);
            }

            _Wait.Close();
            _SerialDevice.Close();
        }

        #endregion

        public void Init()
        {
            var initThread = new Thread(InitPool) { IsBackground = true };
            initThread.Start();
            _DeviceState.Init = true;
        }

        public void ResetNominal()
        {
            _DeviceState.Nominal = 0;
        }

        #region Threads

        private void InitPool()
        {
            try
            {
                Send(CcnetCommand.Poll, null);
                Thread.Sleep(POLLING_INTERVAL);
                Send(CcnetCommand.Poll, null);
                Thread.Sleep(POLLING_INTERVAL);
                Reset();
                Thread.Sleep(POLLING_INTERVAL);
                Send(CcnetCommand.Poll, null);
                Thread.Sleep(POLLING_INTERVAL);
                Send(CcnetCommand.GetStatus, null);
                Thread.Sleep(POLLING_INTERVAL);
                byte[] sec = { 0x00, 0x00, 0x00 };
                Send(CcnetCommand.SetSecurity, sec);
                Thread.Sleep(POLLING_INTERVAL);
                Send(CcnetCommand.Identification, null);
                Thread.Sleep(POLLING_INTERVAL);
                Send(CcnetCommand.Poll, null);
                Thread.Sleep(POLLING_INTERVAL);
                Send(CcnetCommand.Poll, null);
                Thread.Sleep(POLLING_INTERVAL);

                if (_DeviceState.StateCode != CcnetCommand.UnitDisabled)
                {
                    Send(CcnetCommand.Poll, null);
                    Thread.Sleep(POLLING_INTERVAL);

                    // что-то случилось и ничего не запустилось
                    _DeviceState.StateCode = CcnetCommand.FatalError;
                    ProccessStateCode();
                }
            }
            catch (ThreadAbortException)
            {
                Thread.CurrentThread.Abort();
            }
        }

        private void ReadResponseThread()
        {
            try
            {
                byte[] receiveBuffer = new byte[128];
                int bytesRead = 0;
                int bufferIndex = 0;
                int startPacketIndex = 0;

                while (!_Disposing)
                {
                    if (_ParseRead)
                    {
                        try
                        {
                            if (_SerialDevice.CanRead && !_SerialDevice.Closed)
                            {
                                bytesRead = _SerialDevice.Read(receiveBuffer);
                            }

                            if (bytesRead > 0)
                            {
                                _TimedOut = false;
                                AddPacket(receiveBuffer, 0);
                                Array.Clear(receiveBuffer, 0, 128);
                            }
                        }
                        catch (TimeoutException)
                        {
                            _TimedOut = true;
                        }
                        catch (ThreadAbortException exp)
                        {
                            throw exp;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch (ThreadAbortException)
            {
                System.Threading.Thread.CurrentThread.Abort();
            }
        }

        private void StartPooling()
        {
            while (!_Disposing)
            {
                if (_DeviceState.BillEnable)
                {
                    Send(CcnetCommand.Poll, null);
                    Thread.Sleep(POLLING_INTERVAL);
                }

                if (!_StartPool)
                {
                    _Wait.WaitOne();
                }
            }
        }

        #endregion

        #region Packet Utilities

        private CCNETPacket CreatePacket(byte[] buffer, int startIndex)
        {
            byte sync = buffer[startIndex];
            byte addr = buffer[(startIndex + 1) % buffer.Length];
            byte dataLength = buffer[(startIndex + 2) % buffer.Length];
            byte cmd = buffer[(startIndex + 3) % buffer.Length];

            ushort crc = 0;
            byte[] data = null;
            int dataSection = 0;

            if (dataLength >= 7)
            {
                data = new byte[dataLength - 6];
                for (int i = 0; i < (dataLength - 6); i++)
                {
                    data[i] = buffer[(startIndex + 4 + i) % buffer.Length];
                }
                dataSection = data.Length;
            }

            crc |= (ushort)buffer[(startIndex + 4 + dataSection) % buffer.Length];
            crc |= (ushort)(buffer[(startIndex + 4 + dataSection + 1) % buffer.Length] << 8);
            return new CCNETPacket(addr, cmd, data, crc);
        }

        private bool AddPacket(byte[] buffer, int startIndex)
        {
            CCNETPacket packet = CreatePacket(buffer, startIndex);

            if (packet.Cmd != (byte)CcnetCommand.Ok || packet.Cmd != (byte)CcnetCommand.NotMount)
            {
                ParseCommand(packet);
                SendAck(); // Патч для быстрого ответа. Сначала отвечаем, а потом смотрим, чего там не так.
            }

            AddResponsePacket();

            return true;
        }

        private void AddResponsePacket()
        {
            lock (_ResponseSignal)
            {
                Monitor.Pulse(_ResponseSignal);
            }
        }

        private void Send(CcnetCommand cmd, byte[] data)
        {
            ushort crcvalue;

            var packet = new CCNETPacket(0x03, (byte)cmd, data);

            byte[] crc = new byte[packet.Lng - 2];
            crc[0] = packet.Sync;
            crc[1] = packet.Address;
            crc[2] = packet.Lng;
            crc[3] = packet.Cmd;
            if (data != null)
            {
                Array.Copy(data, 0, crc, 4, data.Length);
            }
            crcvalue = CcnetСrcGenerator.GenerateCrc(crc, Convert.ToUInt16(crc.Length));


            byte[] packetXMitBuffer = new byte[packet.Lng];
            packetXMitBuffer[0] = packet.Sync;
            packetXMitBuffer[1] = packet.Address;
            packetXMitBuffer[2] = packet.Lng;
            packetXMitBuffer[3] = packet.Cmd;

            byte[] packetData = packet.Data;

            if (packetData != null && 0 != packetData.Length)
            {
                Array.Copy(packet.Data, 0, packetXMitBuffer, 4, packetData.Length);
            }
            packetXMitBuffer[packet.Lng - 2] = (byte)(crcvalue >> 8);
            packetXMitBuffer[packet.Lng - 1] = (byte)(crcvalue);

            lock (_ResponseSignal)
            {
                _ResponsePacket = null;
                // comPort

                try
                {
                    if (_SerialDevice.CanWrite && !_SerialDevice.Closed)
                    {
                        _SerialDevice.Write(packetXMitBuffer);
                        _SerialDevice.PurgeAll();
                    }
                }
                catch (Exception exp)
                {

                    //throw new Exception(exp.Message.ToString());
                }

                Monitor.Wait(_ResponseSignal, MAX_RESPONSE_TIME);
                Thread.Sleep(0);
            }

            return;
        }

        #endregion

        #region Device Commands

        public void EnableAll()
        {
            byte[] billmask = new byte[6];
            for (int i = 0; i <= 2; i++)
            {
                billmask[i] = 0xff;
            }

            for (int i = 3; i <= 5; i++)
            {
                billmask[i] = 0x00;
            }

            Send(CcnetCommand.EnableDisable, billmask);

            _DeviceState.BillEnable = true;
        }

        public void Disable()
        {
            byte[] disable = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            Send(CcnetCommand.EnableDisable, disable);
            _DeviceState.BillEnable = false;
            _DeviceState.Nominal = 0;

            Thread.Sleep(100);
        }

        public void Reset()
        {
            Send(CcnetCommand.Reset, null);
        }

        private void Return()
        {
            // не реализовано
        }

        public void Poll()
        {
            Send(CcnetCommand.Poll, null);
        }

        private void Hold()
        {
            Send(CcnetCommand.Hold, null);
        }

        private void SendNak()
        {
            Send(CcnetCommand.NotMount, null);
        }

        private void SendAck()
        {
            Send(CcnetCommand.Ok, null);
        }

        #endregion

        #region ParseCommand

        private void ParseCommand(CCNETPacket packet)
        {
            //int length;
            int lenCommand = packet.Lng;

            if (packet.Cmd != 0)
            {
                _DeviceState.StateCode = (CcnetCommand)packet.Cmd;
            }


            if (packet.Data != null && packet.Data.Length > 0)
            {
                _DeviceState.SubStateCode = packet.Data[0];
            }

            switch (_DeviceState.StateCode)
            {
                case CcnetCommand.Wait:
                    _DeviceState.StateCodeOut = CcnetCommand.Ok;
                    break;

                case CcnetCommand.UnitDisabled:
                    _DeviceState.StateCodeOut = CcnetCommand.UnitDisabled;
                    if (_DeviceState.BillEnable)
                    {
                        EnableAll();
                    }
                    break;

                case CcnetCommand.Accepting:
                    _DeviceState.StateCodeOut = CcnetCommand.Accepting;
                    break;

                case CcnetCommand.Initialize:
                    _DeviceState.StateCodeOut = CcnetCommand.Initialize;
                    break;

                case CcnetCommand.StackerFull:
                    _DeviceState.StateCodeOut = CcnetCommand.StackerFull;
                    _DeviceState.FatalError = true;
                    Reset();
                    break;

                case CcnetCommand.StackerOpened:
                    _DeviceState.StateCodeOut = CcnetCommand.StackerOpened;
                    _DeviceState.WasAmount = _DeviceState.Amount;
                    _DeviceState.Amount = 0;
                    break;

                case CcnetCommand.PowerUpWithBillInAcceptor:
                    _DeviceState.StateCodeOut = CcnetCommand.PowerUpWithBillInAcceptor;
                    Reset();
                    break;

                case CcnetCommand.PowerUpWithBillInStacker:
                    _DeviceState.StateCodeOut = CcnetCommand.PowerUpWithBillInStacker;
                    Reset();
                    break;

                case CcnetCommand.PowerUp:
                    _DeviceState.StateCodeOut = CcnetCommand.PowerUp;
                    //Reset();
                    break;

                case CcnetCommand.BillJam:
                    _DeviceState.StateCodeOut = CcnetCommand.BillJam;
                    _DeviceState.FatalError = true;
                    break;

                case CcnetCommand.FatalError:
                    _DeviceState.StateCodeOut = CcnetCommand.FatalError;
                    _DeviceState.FatalError = true;
                    break;

                case CcnetCommand.Cheated:
                    _DeviceState.StateCodeOut = CcnetCommand.Cheated;
                    _DeviceState.FatalError = true;
                    break;

                case CcnetCommand.CasseteBillJam:
                    _DeviceState.StateCodeOut = CcnetCommand.CasseteBillJam;
                    _DeviceState.FatalError = true;
                    break;

                case CcnetCommand.Error:
                    _DeviceState.StateCodeOut = CcnetCommand.Error;
                    _DeviceState.FatalError = true;
                    break;

                case CcnetCommand.Stacking:
                    _DeviceState.StateCodeOut = CcnetCommand.Stacking;
                    break;

                case CcnetCommand.Stacked:
                case CcnetCommand.BillAccepting:
                    _DeviceState.Nominal = 0;
                    _DeviceState.StateCodeOut = CcnetCommand.BillAccepting;
                    _DeviceState.Stacking = true;

                    if (_Currency.ToLower() == "azn")
                    {
                        switch (_DeviceState.SubStateCode)
                        {
                            case 0: // 1 AZN
                                _DeviceState.Nominal = 1;
                                break;

                            case 2: // 5 AZN
                                _DeviceState.Nominal = 5;
                                break;

                            case 3: // 10 AZN
                                _DeviceState.Nominal = 10;
                                break;

                            case 4: // 20 AZN
                                _DeviceState.Nominal = 20;
                                break;

                            case 5: // 50 AZN
                                _DeviceState.Nominal = 50;
                                break;
                        }
                    }

                    if (_DeviceState.Nominal > 0)
                    {
                        _DeviceState.Stacking = false;
                        _DeviceState.WasAmount = _DeviceState.Amount;
                        _DeviceState.Amount += _DeviceState.Nominal;
                    }
                    else
                    {
                        _DeviceState.Stacking = false;
                        Return();
                    }
                    BillStacked(_DeviceState);
                    break;

                case CcnetCommand.BillReturned:
                    _DeviceState.StateCodeOut = CcnetCommand.BillReturned;
                    break;

                default:
                    break;
            }

            ProccessStateCode();
            this.ReadCommand(_DeviceState);
        }

        #endregion

        #region Proccess StateCode

        private void ProccessStateCode()
        {
            switch (_DeviceState.StateCode)
            {
                case CcnetCommand.Idle:
                    _DeviceState.DeviceStateDescription = "Ожидание наличности";
                    break;

                case CcnetCommand.Inactive:
                    _DeviceState.DeviceStateDescription = "Не активен";
                    break;

                case CcnetCommand.ReadyForTransaction:
                    _DeviceState.DeviceStateDescription = "Готов к транзакциям";
                    break;

                case CcnetCommand.Ok:
                    _DeviceState.DeviceStateDescription = "ACK";
                    break;

                case CcnetCommand.NotMount:
                    _DeviceState.DeviceStateDescription = "NAK";
                    break;

                case CcnetCommand.PowerUp:
                case CcnetCommand.PowerUpWithBillInAcceptor:
                case CcnetCommand.PowerUpWithBillInStacker:
                    _DeviceState.DeviceStateDescription = "Включение питания";
                    break;

                case CcnetCommand.FatalError:
                    _DeviceState.DeviceStateDescription = "Фатальная ошибка";
                    break;

                case CcnetCommand.Error:
                    _DeviceState.DeviceStateDescription = "Сбой оборудования";
                    break;

                case CcnetCommand.Initialize:
                    _DeviceState.DeviceStateDescription = "Инициализация";
                    break;

                case CcnetCommand.Wait:
                    _DeviceState.DeviceStateDescription = "Ожидание наличности";
                    break;

                case CcnetCommand.Accepting:
                    _DeviceState.DeviceStateDescription = "Разрешение";
                    break;

                case CcnetCommand.Stacking:
                    _DeviceState.DeviceStateDescription = "Укладка купюры";
                    break;

                case CcnetCommand.BillReturning:
                    _DeviceState.DeviceStateDescription = "Возврат";
                    break;

                case CcnetCommand.UnitDisabled:
                    _DeviceState.DeviceStateDescription = "Готов к работе";
                    break;

                case CcnetCommand.Hold:
                    _DeviceState.DeviceStateDescription = "Удержание";
                    break;

                case CcnetCommand.Busy:
                    _DeviceState.DeviceStateDescription = "Занят";
                    break;

                case CcnetCommand.Dispensing:
                    _DeviceState.DeviceStateDescription = "Выдача";
                    break;

                case CcnetCommand.Unloading:
                    _DeviceState.DeviceStateDescription = "Выгрузка";
                    break;

                case CcnetCommand.CasseseteInsert:
                    _DeviceState.DeviceStateDescription = "Установка кассеты";
                    break;

                case CcnetCommand.Gone:
                    _DeviceState.DeviceStateDescription = "Выдано";
                    break;

                case CcnetCommand.Unload:
                    _DeviceState.DeviceStateDescription = "Разгружено";
                    break;

                case CcnetCommand.UnknownBill:
                    _DeviceState.DeviceStateDescription = "Неверный код купюры";
                    break;

                case CcnetCommand.CasseteInsert:
                    _DeviceState.DeviceStateDescription = "Кассета установлена";
                    break;

                case CcnetCommand.Reset:
                    _DeviceState.DeviceStateDescription = "Неверная команда";
                    break;

                case CcnetCommand.CasseteFull:
                    _DeviceState.DeviceStateDescription = "Кассета переполнена";
                    break;

                case CcnetCommand.CasseteRemoved:
                    _DeviceState.DeviceStateDescription = "Кассета снята";
                    break;

                case CcnetCommand.PaperJam:
                    _DeviceState.DeviceStateDescription = "Купюра застряла";
                    break;

                case CcnetCommand.PaperJam2:
                    _DeviceState.DeviceStateDescription = "Купюра застряла";
                    break;

                case CcnetCommand.Hacking:
                    _DeviceState.DeviceStateDescription = "Взлом";
                    break;

                case CcnetCommand.Pause:
                    _DeviceState.DeviceStateDescription = "Пауза";
                    break;

                case CcnetCommand.BillReceiving:
                    _DeviceState.DeviceStateDescription = "Приём купюры";
                    break;

                case CcnetCommand.BillStacked:
                    _DeviceState.DeviceStateDescription = "Купюра уложена";
                    break;

                case CcnetCommand.BillReturned:
                    _DeviceState.DeviceStateDescription = "Возвращена";
                    break;
            }
        }

        #endregion

        #region CCNETPacket

        internal class CCNETPacket
        {
            private readonly byte _Sync;
            private readonly byte _Address;
            private readonly byte _Cmd;
            private readonly byte _Lng;
            private readonly byte[] _Data;
            private readonly ushort _Crc;

            public CCNETPacket(byte address, byte cmd, byte[] data)
            {
                byte dataLength = 0;

                if (data != null)
                {
                    dataLength = (byte)data.Length;

                    if (dataLength > 250)
                    {
                        return;
                        // пока не реализовано
                    }
                }

                this._Sync = 0x02;
                this._Address = address;
                this._Lng = Convert.ToByte(dataLength + 6);
                this._Cmd = cmd;
                this._Data = data;
            }

            public CCNETPacket(byte address, byte cmd, byte[] data, ushort crc)
            {
                byte dataLength = 0;

                if (data != null)
                {
                    dataLength = (byte)data.Length;

                    if (dataLength > 250)
                    {
                        return;
                        // пока не реализовано
                    }
                }

                this._Sync = 0x02;
                this._Address = address;
                this._Lng = Convert.ToByte(dataLength + 6);
                this._Cmd = cmd;
                this._Data = data;
                this._Crc = crc;
            }

            public byte Sync
            {
                get
                {
                    return this._Sync;
                }
            }

            public byte Address
            {
                get
                {
                    return this._Address;
                }
            }

            public byte Lng
            {
                get
                {
                    return this._Lng;
                }
            }

            public byte Cmd
            {
                get
                {
                    return this._Cmd;
                }
            }

            public byte[] Data
            {
                get
                {
                    return this._Data;
                }
            }

            public ushort Crc
            {
                get
                {
                    return this._Crc;
                }
            }
        }

        #endregion

        #region CCNETCRCGenerator

        internal class CcnetСrcGenerator
        {
            const int POLYNOMINAL = 0x08408;
            static private ushort byteCRC_h;
            static private ushort byteCRC_l;

            public CcnetСrcGenerator()
            {

            }

            public static ushort GenerateCrc(byte[] dataBuf, ushort bufLen)
            {
                ushort i;
                byteCRC_h = byteCRC_l = 0;
                for (i = 0; i < bufLen; i++)
                {
                    calc_crc(dataBuf[i]);
                }
                i = byteCRC_h;
                i <<= 8;
                i += byteCRC_l;
                return i;
            }

            public static byte[] GenerateCrc(byte[] dataBuf, ushort bufLen, bool returnArray)
            {
                ushort i;
                byte[] result = new byte[2];
                byteCRC_h = byteCRC_l = 0;
                for (i = 0; i < bufLen; i++)
                {
                    calc_crc(dataBuf[i]);
                }

                i = byteCRC_h;
                i <<= 8;
                i += byteCRC_l;

                result[0] = Convert.ToByte(i >> 8);
                result[1] = Convert.ToByte(byteCRC_l);
                return result;
            }

            static private void calc_crc(byte mbyte)
            {
                ushort i, c;
                ushort temp_crc;
                byteCRC_h ^= mbyte;
                temp_crc = byteCRC_l;
                temp_crc <<= 8;
                temp_crc |= byteCRC_h;
                for (i = 0; i < 8; i++)
                {
                    c = (ushort)(temp_crc & 0x01);
                    temp_crc >>= 1;
                    if (c != 0) temp_crc ^= POLYNOMINAL;
                }
                byteCRC_l = Convert.ToUInt16(temp_crc >> 8);
                byteCRC_h = Convert.ToUInt16(temp_crc);
            }
        }

        #endregion

    }
}
