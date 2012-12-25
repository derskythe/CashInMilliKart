using System;
using System.Threading;
using CashControlTerminal;

namespace CashInTerminal
{
    public sealed class CCNETDevice : IDisposable
    {
        #region Fields

        static private SerialStream _SerialDevice;
        
        static private bool _StartPool;
        static private bool _ParseRead;

        private const int POLLING_INTERVAL = 1000;
        private const int MAX_RESPONSE_TIME = 250;
        private const int MAX_PACKET_LENGTH = 255;

        private Thread _ResponseReaderThread, _PoolThread, _SendThread, _EventThread;
        private CCNETDeviceState _DeviceState;
        private string _Currency;
        private int _Port;
        private CCNETPortSpeed _PortSpeed;
        private Object _ResponseSignal;
        private CCNETPacket _ResponsePacket;
        private EventWaitHandle _Wait = new AutoResetEvent(false);
        private bool _TimedOut;
        private bool _Disposing = false;

        public delegate void ReadCommandDelegate(CCNETDeviceState e);
        public delegate void BillStackedDelegate(CCNETDeviceState e);
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

                if (_StartPool)
                {
                    _Wait.Set();
                }
            }
        }

        public CCNETDeviceState DeviceState
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

        public CCNETDevice()
        {
            _StartPool = false;           
            _DeviceState = new CCNETDeviceState();
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
            _ResponseReaderThread = new Thread(new ThreadStart(ReadResponseThread));
            _ResponseReaderThread.IsBackground = true;

            // Create the Thread used to send POOL command
            _PoolThread = new Thread(new ThreadStart(StartPooling));
            _PoolThread.IsBackground = true;

            _SerialDevice = new SerialStream();
            
        }

        public void Open(int port, CCNETPortSpeed speed)
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
            catch(Exception e)
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

            if(_ResponseReaderThread.ThreadState == (ThreadState.Background | ThreadState.Unstarted))
            {
                _ResponseReaderThread.Start();
            }

            if(_PoolThread.ThreadState == (ThreadState.Background | ThreadState.Unstarted))
            {
                _PoolThread.Start();
            }
            _ParseRead = true;
        }

        public void Close()
        {
            if(!_SerialDevice.Closed)
            {
                _SerialDevice.Close();                
            }

            if(_ResponseReaderThread.ThreadState == ThreadState.Running)
            {
                _ParseRead = false;
            }

            if(_PoolThread.ThreadState == ThreadState.Running)
            {
                _StartPool = false;
            }
        }

        public void Dispose()
        {
            _Disposing = true;
            if (_ResponseReaderThread != null && _ResponseReaderThread.IsAlive )
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
            Thread initThread = new Thread(new ThreadStart(InitPool));
            initThread.IsBackground = true;
            initThread.Start();
            _DeviceState.Init = true;
        }

        public void ResetNominal()
        {
            this._DeviceState.Nominal = 0;
        }

        #region Threads

        private void InitPool()
        {
            try
            {
                Send(CCNETCommand.Poll, null);
                Thread.Sleep(POLLING_INTERVAL);
                Send(CCNETCommand.Poll, null);
                Thread.Sleep(POLLING_INTERVAL);
                Reset();
                Thread.Sleep(POLLING_INTERVAL);
                Send(CCNETCommand.Poll, null);
                Thread.Sleep(POLLING_INTERVAL);
                Send(CCNETCommand.GetStatus, null);
                Thread.Sleep(POLLING_INTERVAL);
                byte[] sec = {0x00, 0x00, 0x00};
                Send(CCNETCommand.SetSecurity, sec);
                Thread.Sleep(POLLING_INTERVAL);
                Send(CCNETCommand.Identification, null);
                Thread.Sleep(POLLING_INTERVAL);
                Send(CCNETCommand.Poll, null);
                Thread.Sleep(POLLING_INTERVAL);
                Send(CCNETCommand.Poll, null);
                Thread.Sleep(POLLING_INTERVAL);

                if (_DeviceState.StateCode != CCNETCommand.UnitDisabled)
                {
                    Send(CCNETCommand.Poll, null);
                    Thread.Sleep(POLLING_INTERVAL);

                    // ���-�� ��������� � ������ �� �����������
                    _DeviceState.StateCode = CCNETCommand.FatalError;
                    ProccessStateCode();
                }
            }
            catch (ThreadAbortException)
            {
                System.Threading.Thread.CurrentThread.Abort();
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

                while(!_Disposing)
                {
                    if(_ParseRead)
                    {
                        try
                        {
                            if(_SerialDevice.CanRead && !_SerialDevice.Closed)
                            {
                                bytesRead = _SerialDevice.Read(receiveBuffer);
                            }

                            if(bytesRead > 0)
                            {
                                _TimedOut = false;
                                AddPacket(receiveBuffer, 0);
                                Array.Clear(receiveBuffer, 0, 128);
                            }
                        }
                        catch(TimeoutException)
                        {
                            _TimedOut = true;
                        }
                        catch(ThreadAbortException exp)
                        {
                            throw exp;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch(ThreadAbortException)
            {
                System.Threading.Thread.CurrentThread.Abort();
            }
        }

        private void StartPooling()
        {
            while(!_Disposing)
            {
                if (_DeviceState.BillEnable)
                {
                    Send(CCNETCommand.Poll, null);
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
                for (int i = 0; i < (dataLength-6); i++)
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

            if(packet.Cmd != (byte)CCNETCommand.Ok || packet.Cmd != (byte)CCNETCommand.NotMount)
            {
                SendACK(); // ���� ��� �������� ������. ������� ��������, � ����� �������, ���� ��� �� ���.
                ParseCommand(packet);
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

        private void Send(CCNETCommand cmd, byte[] data)
        {
            ushort crcvalue;

            CCNETPacket packet = new CCNETPacket(0x03, (byte)cmd, data);

            byte[] crc = new byte[packet.Lng - 2];
            crc[0] = packet.Sync;
            crc[1] = packet.Address;
            crc[2] = packet.Lng;
            crc[3] = packet.Cmd;
            if (data != null)
            {
                Array.Copy(data, 0, crc, 4, data.Length);
            }
            crcvalue = CCNETCRCGenerator.GenerateCRC(crc, Convert.ToUInt16(crc.Length));


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
                    if(_SerialDevice.CanWrite && !_SerialDevice.Closed)
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
            //byte[] billmask = new byte[] { 0xFD, 0x7F, 0x7F };
            byte[] billmask = new byte[] { 0xFF, 0xFF, 0xFF };
            //for( int i = 0; i <= 2; i++ )
            //{
            //    billmask[i] = 0xff;
            //}

            //for (int i = 3; i <= 5; i++)
            //{
            //    billmask[i] = 0x00;
            //}

            Send(CCNETCommand.EnableDisable, billmask);

            _DeviceState.BillEnable = true;
        }

        public void Disable()
        {
            byte[] disable = {0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
            Send(CCNETCommand.EnableDisable, disable);
            _DeviceState.BillEnable = false;
            _DeviceState.Nominal = 0;

            Thread.Sleep(100);
        }

        public void Reset()
        {
            Send(CCNETCommand.Reset, null);
        }

        private void Return()
        {
            // �� �����������
        }

        public void Poll()
        {
            Send(CCNETCommand.Poll, null);
        }

        private void Hold()
        {
            Send(CCNETCommand.Hold, null);
        }

        private void SendNAK()
        {
            Send(CCNETCommand.NotMount, null);
        }

        private void SendACK()
        {
            Send(CCNETCommand.Ok, null);
        }

        #endregion

        #region ParseCommand

        private void ParseCommand(CCNETPacket packet)
        {
            //int length;
            int len_command = packet.Lng;

            if (packet.Cmd != 0)
            {
                _DeviceState.StateCode = (CCNETCommand)packet.Cmd;
            }

            
            if (packet.Data != null && packet.Data.Length > 0)
            {
                _DeviceState.SubStateCode = packet.Data[0];
            }

            switch (_DeviceState.StateCode)
            {
                case CCNETCommand.Wait:
                    _DeviceState.StateCodeOut = CCNETCommand.Ok;
                    break;

                case CCNETCommand.UnitDisabled:
                    _DeviceState.StateCodeOut = CCNETCommand.UnitDisabled;
                    if (_DeviceState.BillEnable) 
                    {
                        EnableAll();
                    }
                    break;

                case CCNETCommand.Accepting:
                    _DeviceState.StateCodeOut = CCNETCommand.Accepting;
                    break;

                case CCNETCommand.Initialize:
                    _DeviceState.StateCodeOut = CCNETCommand.Initialize;
                    break;

                case CCNETCommand.StackerFull:
                    _DeviceState.StateCodeOut = CCNETCommand.StackerFull;
                    _DeviceState.FatalError = true;
                    Reset();
                    break;

                case CCNETCommand.StackerOpened:
                    _DeviceState.StateCodeOut = CCNETCommand.StackerOpened;
                    _DeviceState.WasAmount = _DeviceState.Amount;
                    _DeviceState.Amount = 0;
                    break;

                case CCNETCommand.PowerUpWithBillInAcceptor:
                    _DeviceState.StateCodeOut = CCNETCommand.PowerUpWithBillInAcceptor;
                    Reset();
                    break;

                case CCNETCommand.PowerUpWithBillInStacker:
                    _DeviceState.StateCodeOut = CCNETCommand.PowerUpWithBillInStacker;
                    Reset();
                    break;

                case CCNETCommand.PowerUp:
                    _DeviceState.StateCodeOut = CCNETCommand.PowerUp;
                    //Reset();
                    break;

                case CCNETCommand.BillJam:
                    _DeviceState.StateCodeOut = CCNETCommand.BillJam;
                    _DeviceState.FatalError = true;
                    break;

                case CCNETCommand.FatalError:
                    _DeviceState.StateCodeOut = CCNETCommand.FatalError;
                    _DeviceState.FatalError = true;
                    break;

                case CCNETCommand.Cheated:
                    _DeviceState.StateCodeOut = CCNETCommand.Cheated;
                    _DeviceState.FatalError = true;
                    break;

                case CCNETCommand.CasseteBillJam:
                    _DeviceState.StateCodeOut = CCNETCommand.CasseteBillJam;
                    _DeviceState.FatalError = true;
                    break;

                case CCNETCommand.Error:
                    _DeviceState.StateCodeOut = CCNETCommand.Error;
                    _DeviceState.FatalError = true;
                    break;

                case CCNETCommand.Stacking:
                    _DeviceState.StateCodeOut = CCNETCommand.Stacking;
                    break;

                case CCNETCommand.Stacked:
                case CCNETCommand.BillAccepting:
                    _DeviceState.Nominal = 0;
                    _DeviceState.StateCodeOut = CCNETCommand.BillAccepting;
                    _DeviceState.Stacking = true;

                    if ( _Currency.ToLower() == "azn" ) 
                    {
                        switch (_DeviceState.SubStateCode)
                        {
                            //case CcnetBillTypes.Azn1: // 1 AZN
                            //    _DeviceState.Nominal = 1;
                            //    break;

                            case CcnetBillTypes.Azn5: // 5 AZN
                                _DeviceState.Nominal = 5;
                                break;

                            case CcnetBillTypes.Azn10: // 10 AZN
                                _DeviceState.Nominal = 10;
                                break;

                            case CcnetBillTypes.Azn20: // 20 AZN
                                _DeviceState.Nominal = 20;
                                break;

                            case CcnetBillTypes.Azn50: // 50 AZN
                                _DeviceState.Nominal = 50;
                                break;

                            case CcnetBillTypes.Azn100:
                                _DeviceState.Nominal = 100;
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

                case CCNETCommand.BillReturned:
                    _DeviceState.StateCodeOut = CCNETCommand.BillReturned;
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
                case CCNETCommand.Idle:
                    _DeviceState.DeviceStateDescription = "�������� ����������";
                    break;

                case CCNETCommand.Inactive:
                    _DeviceState.DeviceStateDescription = "�� �������";
                    break;

                case CCNETCommand.ReadyForTransaction:
                    _DeviceState.DeviceStateDescription = "����� � �����������";
                    break;

                case CCNETCommand.Ok:
                    _DeviceState.DeviceStateDescription = "ACK";
                    break;

                case CCNETCommand.NotMount:
                    _DeviceState.DeviceStateDescription = "NAK";
                    break;

                case CCNETCommand.PowerUp:
                case CCNETCommand.PowerUpWithBillInAcceptor:
                case CCNETCommand.PowerUpWithBillInStacker:
                    _DeviceState.DeviceStateDescription = "��������� �������";
                    break;

                case CCNETCommand.FatalError:
                    _DeviceState.DeviceStateDescription = "��������� ������";
                    break;

                case CCNETCommand.Error:
                    _DeviceState.DeviceStateDescription = "���� ������������";
                    break;

                case CCNETCommand.Initialize:
                    _DeviceState.DeviceStateDescription = "�������������";
                    break;

                case CCNETCommand.Wait:
                    _DeviceState.DeviceStateDescription = "�������� ����������";
                    break;

                case CCNETCommand.Accepting:
                    _DeviceState.DeviceStateDescription = "����������";
                    break;

                case CCNETCommand.Stacking:
                    _DeviceState.DeviceStateDescription = "������� ������";
                    break;

                case CCNETCommand.BillReturning:
                    _DeviceState.DeviceStateDescription = "�������";
                    break;

                case CCNETCommand.UnitDisabled:
                    _DeviceState.DeviceStateDescription = "����� � ������";
                    break;

                case CCNETCommand.Hold:
                    _DeviceState.DeviceStateDescription = "���������";
                    break;

                case CCNETCommand.Busy:
                    _DeviceState.DeviceStateDescription = "�����";
                    break;

                case CCNETCommand.Dispensing:
                    _DeviceState.DeviceStateDescription = "������";
                    break;

                case CCNETCommand.Unloading:
                    _DeviceState.DeviceStateDescription = "��������";
                    break;

                case CCNETCommand.CasseseteInsert:
                    _DeviceState.DeviceStateDescription = "��������� �������";
                    break;

                case CCNETCommand.Gone:
                    _DeviceState.DeviceStateDescription = "������";
                    break;

                case CCNETCommand.Unload:
                    _DeviceState.DeviceStateDescription = "����������";
                    break;

                case CCNETCommand.UnknownBill:
                    _DeviceState.DeviceStateDescription = "�������� ��� ������";
                    break;

                case CCNETCommand.CasseteInsert:
                    _DeviceState.DeviceStateDescription = "������� �����������";
                    break;

                case CCNETCommand.Reset:
                    _DeviceState.DeviceStateDescription = "�������� �������";
                    break;

                case CCNETCommand.CasseteFull:
                    _DeviceState.DeviceStateDescription = "������� �����������";
                    break;

                case CCNETCommand.CasseteRemoved:
                    _DeviceState.DeviceStateDescription = "������� �����";
                    break;

                case CCNETCommand.PaperJam:
                    _DeviceState.DeviceStateDescription = "������ ��������";
                    break;

                case CCNETCommand.PaperJam2:
                    _DeviceState.DeviceStateDescription = "������ ��������";
                    break;

                case CCNETCommand.Hacking:
                    _DeviceState.DeviceStateDescription = "�����";
                    break;

                case CCNETCommand.Pause:
                    _DeviceState.DeviceStateDescription = "�����";
                    break;

                case CCNETCommand.BillReceiving:
                    _DeviceState.DeviceStateDescription = "���� ������";
                    break;

                case CCNETCommand.BillStacked:
                    _DeviceState.DeviceStateDescription = "������ �������";
                    break;

                case CCNETCommand.BillReturned:
                    _DeviceState.DeviceStateDescription = "����������";
                    break;
            }
        }

        #endregion

        #region CCNETPacket

        internal class CCNETPacket
        {
            private byte sync;
            private byte address;
            private byte cmd;
            private byte lng;
            private byte[] data;
            private ushort crc;

            public CCNETPacket(byte address, byte cmd, byte[] data)
            {
                byte dataLength = 0;

                if (data != null)
                {
                    dataLength = (byte)data.Length;

                    if (dataLength > 250)
                    {
                        return;
                        // ���� �� �����������
                    }
                }

                this.sync = 0x02;
                this.address = address;
                this.lng = Convert.ToByte(dataLength + 6);
                this.cmd = cmd;
                this.data = data;
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
                        // ���� �� �����������
                    }
                }

                this.sync = 0x02;
                this.address = address;
                this.lng = Convert.ToByte(dataLength + 6);
                this.cmd = cmd;
                this.data = data;
                this.crc = crc;
            }

            public byte Sync
            {
                get
                {
                    return this.sync;
                }
            }

            public byte Address
            {
                get
                {
                    return this.address;
                }
            }

            public byte Lng
            {
                get
                {
                    return this.lng;
                }
            }

            public byte Cmd
            {
                get
                {
                    return this.cmd;
                }
            }

            public byte[] Data
            {
                get
                {
                    return this.data;
                }
            }

            public ushort CRC
            {
                get
                {
                    return this.crc;
                }
            }
        }

        #endregion

        #region CCNETCRCGenerator

        internal class CCNETCRCGenerator
        {
            const int POLYNOMINAL = 0x08408;
            static private ushort byteCRC_h;
            static private ushort byteCRC_l;

            public CCNETCRCGenerator()
            {

            }

            public static ushort GenerateCRC(byte[] DataBuf, ushort BufLen)
            {
                ushort i;
                byteCRC_h = byteCRC_l = 0;
                for (i = 0; i < BufLen; i++)
                {
                    calc_crc(DataBuf[i]);
                }
                i = byteCRC_h;
                i <<= 8;
                i += byteCRC_l;
                return i;
            }

            public static byte[] GenerateCRC(byte[] DataBuf, ushort BufLen, bool returnArray)
            {
                ushort i;
                byte[] result = new byte[2];
                byteCRC_h = byteCRC_l = 0;
                for (i = 0; i < BufLen; i++)
                {
                    calc_crc(DataBuf[i]);
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

    #region CCNETCommand

    public enum CCNETCommand
    {
        /// <summary>
        /// Ok
        /// </summary>
        Ok = 0x00,                          // Ok
        /// <summary>
        /// �������� ����������
        /// </summary>
        Idle = 0x06,                        // �������� ����������
        /// <summary>
        /// �� �������
        /// </summary>
        Inactive = 0x01,                    // �� �������
        /// <summary>
        /// ����� � �����������
        /// </summary>
        ReadyForTransaction = 0x07,         // ����� � �����������
        /// <summary>
        /// ��������
        /// </summary>
        Wait = 0x14,                        // ��������
        /// <summary>
        /// �������� ������
        /// </summary>
        GetStatus = 0x31,                   // �������� ������
        /// <summary>
        /// ���������� ���� ������������
        /// </summary>
        SetSecurity = 0x32,                 // ���������� ���� ������������
        /// <summary>
        /// �������������
        /// </summary>
        Identification = 0x37,              // �������������
        /// <summary>
        /// ����������
        /// </summary>
        UnitDisabled = 0x19,                // ����������
        /// <summary>
        /// ��������������
        /// </summary>
        Accepting = 0x15,                   // ��������������
        /// <summary>
        /// �������������
        /// </summary>
        Initialize = 0x13,                  // �������������
        /// <summary>
        /// ������ ������
        /// </summary>
        StackerFull = 0x41,                 // ������ ������
        /// <summary>
        /// ������ �������
        /// </summary>
        StackerOpened = 0x42,               // ������ �������
        /// <summary>
        /// �������� ������������� with BILL
        /// </summary>
        PowerUpWithBillInAcceptor = 0x11,   // �������� ������������� with BILL
        /// <summary>
        /// �������� ������������� with BILL
        /// </summary>
        PowerUpWithBillInStacker = 0x12,    // �������� ������������� with BILL
        /// <summary>
        /// �������� �������������
        /// </summary>
        PowerUp = 0x10,                     // �������� �������������
        /// <summary>
        /// ������ ������
        /// </summary>
        BillJam = 0x43,                     // ������ ������
        /// <summary>
        /// fatal error
        /// </summary>
        FatalError = 0x1C,                  // fatal error
        /// <summary>
        /// cheated, �����
        /// </summary>
        Cheated = 0x45,                     // cheated, �����
        /// <summary>
        /// cassete jammed
        /// </summary>
        CasseteBillJam = 0x44,              // cassete jammed
        /// <summary>
        /// ���� ������������
        /// </summary>
        Error = 0x47,                       // ���� ������������
        /// <summary>
        /// �� �����
        /// </summary>
        NotMount = 0xFF,                    // �� �����
        /// <summary>
        /// ����� ������� �� ���������
        /// </summary>
        BillAccepting = 0x80,               // ����� ������� �� ���������
        /// <summary>
        /// stacking, �������� ������ � ��������������
        /// </summary>
        Stacking = 0x17,                    // stacking, �������� ������ � ��������������
        /// <summary>
        /// stacked, ������ �������
        /// </summary>
        Stacked = 0x81,                     // stacked, ������ �������
        /// <summary>
        /// ������� ������
        /// </summary>
        BillReturning = 0x18,                   // ������� ������
        /// <summary>
        /// ���������, HOLD
        /// </summary>
        Hold = 0x1A,                        // ���������, HOLD
        /// <summary>
        /// �����
        /// </summary>
        Busy = 0x1B,                        // �����
        /// <summary>
        /// ������
        /// </summary>
        Dispensing = 0x1D,                  // ������
        /// <summary>
        /// �������� ��������� �����
        /// </summary>
        Unloading = 0x1E,                   // ��������
        /// <summary>
        /// ��������� �������
        /// </summary>
        CasseseteInsert = 0x21,             // ��������� �������
        /// <summary>
        /// "������"
        /// </summary>
        Gone = 0x25,                        // ������
        /// <summary>
        /// ����������
        /// </summary>
        Unload = 0x26,                      // ����������
        /// <summary>
        /// �������� ��� ������
        /// </summary>
        UnknownBill = 0x28,                 // �������� ��� ������
        /// <summary>
        /// ������� �����������
        /// </summary>
        CasseteInsert = 0x29,               // ������� �����������
        /// <summary>
        /// ������� �����������
        /// </summary>
        CasseteFull = 0x41,                 // ������� �����������
        /// <summary>
        /// ������� �����
        /// </summary>
        CasseteRemoved = 0x42,              // ������� �����
        /// <summary>
        /// ������ ��������
        /// </summary>
        PaperJam = 0x43,                    // ������ ��������
        /// <summary>
        /// ������ ��������
        /// </summary>
        PaperJam2 = 0x44,                   // ������ ��������
        /// <summary>
        /// �����
        /// </summary>
        Hacking = 0x45,                     // �����
        /// <summary>
        /// �����
        /// </summary>
        Pause = 0x46,                       // �����
        /// <summary>
        /// ���� ������
        /// </summary>
        BillReceiving = 0x80,               // ���� ������
        /// <summary>
        /// ������ �������
        /// </summary>
        BillStacked = 0x81,                 // ������ �������
        /// <summary>
        /// ������ ����������
        /// </summary>
        BillReturned = 0x82,                // ������ ����������
        /// <summary>
        /// ������� �� ���������� ������ ������� �� ���������
        /// </summary>
        EnableDisable = 0x34,               // ���������
        /// <summary>
        /// ������������
        /// </summary>
        Reset = 0x30,                       // ������������
        /// <summary>
        /// POLL. Ping
        /// </summary>
        Poll = 0x33                         // POLL. Ping
    }

    #endregion             

    #region CCNETDeviceState

    public class CCNETDeviceState
    {
        private CCNETCommand _StateCode; // �����. ���������
        /// <summary>
        /// �����. ���������
        /// </summary>
        public CCNETCommand StateCode
        {
            get
            {
                return _StateCode;
            }
            set
            {
                _StateCode = value;
            }
        }
        private byte _SubStateCode; // ������ ���������
        /// <summary>
        /// ������ ���������
        /// </summary>
        public byte SubStateCode
        {
            get
            {
                return _SubStateCode;
            }
            set
            {
                _SubStateCode = value;
            }
        }
        private CCNETCommand _StateCodeOut; // ����. ���������

        /// <summary>
        /// ����. ���������
        /// </summary>
        public CCNETCommand StateCodeOut
        {
            get
            {
                return _StateCodeOut;
            }
            set
            {
                _StateCodeOut = value;
            }
        }
        private bool _BillEnable; // �� �������� � ��������� ��������

        /// <summary>
        /// �� �������� � ��������� ��������
        /// </summary>
        public bool BillEnable
        {
            get
            {
                return _BillEnable;
            }
            set
            {
                _BillEnable = value;
            }
        }
        private bool _AcceptEnable; // �� ���� �����

        /// <summary>
        /// �� ���� �����
        /// </summary>
        public bool AcceptEnable
        {
            get
            {
                return _AcceptEnable;
            }
            set
            {
                _AcceptEnable = value;
            }
        }
        private bool _FatalError;  // ��������� ������. ���������� �� ����� ��������

        /// <summary>
        /// ��������� ������. ���������� �� ����� ��������
        /// </summary>
        public bool FatalError
        {
            get
            {
                return _FatalError;
            }
            set
            {
                _FatalError = value;
            }
        }
        private int _Amount; // ����� � ���������������

        /// <summary>
        /// ����� � ���������������
        /// </summary>
        public int Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                _Amount = value;
            }
        }
        private int _WasAmount; // ���� � ���������������

        /// <summary>
        /// ���� � ���������������
        /// </summary>
        public int WasAmount
        {
            get
            {
                return _WasAmount;
            }
            set
            {
                _WasAmount = value;
            }
        }
        private int _Nominal; // ������� ��������� �������� ������

        /// <summary>
        /// ������� ��������� �������� ������
        /// </summary>
        public int Nominal
        {
            get
            {
                return _Nominal;
            }
            set
            {
                _Nominal = value;
            }
        }
        private string _DeviceStateDescription; // �������� ���������

        /// <summary>
        /// �������� ���������
        /// </summary>
        public string DeviceStateDescription
        {
            get
            {
                return _DeviceStateDescription;
            }
            set
            {
                _DeviceStateDescription = value;
            }
        }
        private string _SubDeviceStateDescription; // �������������� �������� ���������

        /// <summary>
        /// �������������� �������� ���������
        /// </summary>
        public string SubDeviceStateDescription
        {
            get
            {
                return _SubDeviceStateDescription;
            }
            set
            {
                _SubDeviceStateDescription = value;
            }
        }
        private bool _Stacking; // � ������ ������ ������.


        /// <summary>
        /// � ������ ������ ������
        /// </summary>
        public bool Stacking
        {
            get
            {
                return _Stacking;
            }
            set
            {
                _Stacking = value;
            }
        }
        private bool _Init; // �� ���������

        /// <summary>
        /// �� ���������
        /// </summary>
        public bool Init
        {
            get
            {
                return _Init;
            }
            set
            {
                _Init = value;
            }
        }

        public override string ToString()
        {
            return
                string.Format(
                    "StateCode: {0}, SubStateCode: {1}, StateCodeOut: {2}, BillEnable: {3}, AcceptEnable: {4}, FatalError: {5}, Amount: {6}, WasAmount: {7}, Nominal: {8}, DeviceStateDescription: {9}, SubDeviceStateDescription: {10}, Stacking: {11}, Init: {12}",
                    _StateCode, _SubStateCode, _StateCodeOut, _BillEnable, _AcceptEnable, _FatalError, _Amount,
                    _WasAmount, _Nominal, _DeviceStateDescription, _SubDeviceStateDescription, _Stacking, _Init);
        }
    }

    #endregion

    #region CCNETPortSpeed

    public enum CCNETPortSpeed
    {
        B_9600 = 9600,
        B_19200 = 19200
    }

    #endregion
}
