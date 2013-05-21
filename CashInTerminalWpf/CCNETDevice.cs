using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CashInTerminalWpf.Enums;
using Containers;
using Containers.CashCode;
using NLog;

namespace CashInTerminalWpf
{
    public sealed class CCNETDevice : IDisposable
    {
        #region Fields

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        static private SerialStream _SerialDevice;

        static private bool _StartPool;
        static private bool _ParseRead;

        private const int POLLING_INTERVAL = 1000;
        /*
                private const int MAX_RESPONSE_TIME = 250;
        */
        /*
                private const int MAX_PACKET_LENGTH = 255;
        */

        private readonly Thread _ResponseReaderThread;
        private readonly Thread _PoolThread;
        private readonly Thread _SendThread;
        //private Thread _EventThread;
        private readonly CCNETDeviceState _DeviceState;
        private String _CurrentCurrency;
        private int _Port;
        private CCNETPortSpeed _PortSpeed;
        private readonly Queue<byte[]> _ResponseSignal = new Queue<byte[]>();
        //private CCNETPacket _ResponsePacket;
        private readonly EventWaitHandle _Wait = new AutoResetEvent(false);
        private readonly Timer _BackgroundPingTimer;
        private bool _TimedOut;
        private bool _Disposing;
        private CCNETControllerCommand _CurrentCommand;
        private readonly Dictionary<int, KeyValuePair<int, String>> _BillTable = new Dictionary<int, KeyValuePair<int, String>>(10);
        /*
                private bool _StartSend;
        */

        public delegate void ReadCommandHandler(CCNETDeviceState e);

        public delegate void BillStackedHandler(CCNETDeviceState e);

        public delegate void BillRejectHandler(CCNETDeviceState e);

        public delegate void StartCompletedHandler(CCNETDeviceState e);

        public delegate void GetBillsHandler(CCNETDeviceState e);

        public event ReadCommandHandler ReadCommand = delegate { };
        public event BillStackedHandler BillStacked = delegate { };
        public event BillRejectHandler BillRejected = delegate { };
        public event StartCompletedHandler StartCompleted = delegate { };
        public event GetBillsHandler GetBills = delegate { };

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

        public String CurrentCurrency
        {
            get
            {
                return _CurrentCurrency;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _CurrentCurrency = value.ToUpperInvariant();
                    if (_CurrentCurrency != Currencies.Azn && _CurrentCurrency != Currencies.Eur &&
                        _CurrentCurrency != Currencies.Usd)
                    {
                        _CurrentCurrency = Currencies.Azn;
                    }
                }
                else
                {
                    _CurrentCurrency = Currencies.Azn;
                }
            }
        }

        public bool TimedOut
        {
            get { return _TimedOut; }
            set { _TimedOut = value; }
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
            //_Currency = "azn";
            _ResponseSignal = new Queue<byte[]>();

            // Create the Thread used to read responses
            _ResponseReaderThread = new Thread(ReadResponseThread) { IsBackground = true };

            // Create the Thread used to send POOL command
            _PoolThread = new Thread(StartPooling) { IsBackground = true };

            _SendThread = new Thread(SendThread);

            _SerialDevice = new SerialStream();
            _BackgroundPingTimer = new Timer(BackgroundPingTimer, null, 0, 60 * 1000);
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
            catch (Exception exp)
            {
                // "Error: " + e.Message, 
                Log.ErrorException(exp.Message, exp);
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

            _SendThread.Start();
            _ParseRead = true;
        }

        public void Close()
        {
            if (_ResponseReaderThread.ThreadState == ThreadState.Running)
            {
                _ParseRead = false;
            }

            if (_PoolThread.ThreadState == ThreadState.Running)
            {
                _StartPool = false;
            }

            if (!_SerialDevice.Closed)
            {
                _SerialDevice.Close();
            }
        }

        public void Dispose()
        {
            _Disposing = true;

            _BackgroundPingTimer.Dispose();

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
                Send(CCNETControllerCommand.Poll, null);
                Thread.Sleep(POLLING_INTERVAL);
                Send(CCNETControllerCommand.Poll, null);
                Thread.Sleep(POLLING_INTERVAL);
                Reset();
                Thread.Sleep(POLLING_INTERVAL);
                Send(CCNETControllerCommand.Poll, null);
                Thread.Sleep(POLLING_INTERVAL);
                Send(CCNETControllerCommand.GetStatus, null);
                Thread.Sleep(POLLING_INTERVAL);
                byte[] sec = { 0x00, 0x00, 0x00 };
                Send(CCNETControllerCommand.SetSecurity, sec);
                Thread.Sleep(POLLING_INTERVAL);
                Send(CCNETControllerCommand.Identification, null);
                Thread.Sleep(POLLING_INTERVAL);
                Send(CCNETControllerCommand.Poll, null);
                Thread.Sleep(POLLING_INTERVAL);
                Send(CCNETControllerCommand.Poll, null);
                Thread.Sleep(POLLING_INTERVAL);

                if (_DeviceState.StateCode != CCNETResponseStatus.UnitDisabled)
                {
                    Send(CCNETControllerCommand.Poll, null);
                    Thread.Sleep(POLLING_INTERVAL);
                    _DeviceState.StateCode = CCNETResponseStatus.Error;
                    ProccessStateCode();
                }

                Thread.Sleep(POLLING_INTERVAL);
                Thread.Sleep(POLLING_INTERVAL);
                Send(CCNETControllerCommand.GetBillTable, null);
                Thread.Sleep(POLLING_INTERVAL);
                Send(CCNETControllerCommand.Poll, null);
                Thread.Sleep(POLLING_INTERVAL);
                Thread.Sleep(POLLING_INTERVAL);
            }
            catch (ThreadAbortException exp)
            {
                Log.Error(exp.Message);
                //Thread.CurrentThread.Abort();
            }

            StartCompleted(_DeviceState);
        }

        private void ReadResponseThread()
        {
            try
            {
                byte[] receiveBuffer = Enumerable.Repeat((byte)0x0, 128).ToArray();
                int bytesRead = 0;
                //int bufferIndex = 0;
                //int startPacketIndex = 0;

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
                                receiveBuffer = Enumerable.Repeat((byte)0x0, 128).ToArray();
                            }
                        }
                        catch (TimeoutException exp)
                        {
                            Log.Error(exp.Message);
                            _TimedOut = true;
                        }
                        catch (ThreadAbortException exp)
                        {
                            Log.Error(exp.Message);
                        }
                        catch (Exception exp)
                        {
                            Log.ErrorException(exp.Message, exp);
                        }
                    }
                }
            }
            catch (ThreadAbortException exp)
            {
                Log.Error(exp.Message);
                //Thread.CurrentThread.Abort();
            }
        }

        private void StartPooling()
        {
            while (!_Disposing)
            {
                if (_DeviceState.BillEnable)
                {
                    Send(CCNETControllerCommand.Poll, null);
                    Thread.Sleep(POLLING_INTERVAL);
                }

                if (!_StartPool)
                {
                    _Wait.WaitOne();
                }
            }
        }

        private void BackgroundPingTimer(object state)
        {
            if (_DeviceState.BillEnable || _Disposing || !_DeviceState.Init)
            {
                return;
            }

            try
            {
                Send(CCNETControllerCommand.Poll, null);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        #endregion

        #region Packet Utilities

        private CCNETPacket CreatePacket(byte[] buffer, int startIndex)
        {
            //byte sync = buffer[startIndex];
            if (buffer == null)
            {
                return null;
                //Log.Debug(Encoding.ASCII.GetString(buffer));
            }
            byte addr = buffer[(startIndex + 1) % buffer.Length];
            byte dataLength = buffer[(startIndex + 2) % buffer.Length];

            byte cmd;
            int offset;

            switch (_CurrentCommand)
            {
                case CCNETControllerCommand.Identification:
                    cmd = (byte)CCNETResponseStatus.Identification;
                    offset = 3;
                    break;

                case CCNETControllerCommand.GetBillTable:
                    cmd = (byte)CCNETResponseStatus.BillTable;
                    offset = 3;
                    break;

                default:
                    cmd = buffer[(startIndex + 3) % buffer.Length];
                    offset = 4;
                    break;

            }

            ushort crc = 0;
            byte[] data = null;
            int dataSection = 0;

            if (dataLength >= 7)
            {
                data = new byte[dataLength - 6];
                for (int i = 0; i < (dataLength - 6); i++)
                {
                    data[i] = buffer[(startIndex + offset + i) % buffer.Length];
                }
                dataSection = data.Length;
            }

            crc |= buffer[(startIndex + offset + dataSection) % buffer.Length];
            crc |= (ushort)(buffer[(startIndex + offset + dataSection + 1) % buffer.Length] << 8);

            return new CCNETPacket(addr, cmd, data, crc);
        }

        private void AddPacket(byte[] buffer, int startIndex)
        {
            if (buffer == null)
            {
                return;
            }

            CCNETPacket packet = CreatePacket(buffer, startIndex);

            if (packet.Cmd != (byte)CCNETResponseStatus.Ok || packet.Cmd != (byte)CCNETResponseStatus.NotMount)
            {
                SendAck();
                ParseCommand(packet);
            }

            //AddResponsePacket();
        }
        //private void AddResponsePacket()
        //{
        //    lock (_ResponseSignal)
        //    {
        //        Monitor.Pulse(_ResponseSignal);
        //    }
        //}

        //private void AddResponsePacket()
        //{
        //    lock (_ResponseSignal)
        //    {
        //        Monitor.Pulse(_ResponseSignal);
        //    }
        //}

        private void Send(CCNETControllerCommand cmd, byte[] data)
        {
            var packet = new CCNETPacket(0x03, (byte)cmd, data);

            var crc = new byte[packet.Lng - 2];
            crc[0] = packet.Sync;
            crc[1] = packet.Address;
            crc[2] = packet.Lng;
            crc[3] = packet.Cmd;
            if (data != null)
            {
                Array.Copy(data, 0, crc, 4, data.Length);
            }
            ushort crcvalue = CCNETCRCGenerator.GenerateCrc(crc, Convert.ToUInt16(crc.Length));


            var packetXMitBuffer = new byte[packet.Lng];
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

            //lock (_ResponseSignal)
            //{

            _CurrentCommand = cmd;
            _ResponseSignal.Enqueue(packetXMitBuffer);
            //_ResponsePacket = null;
            // comPort



            //Monitor.Wait(_ResponseSignal, MAX_RESPONSE_TIME);
            //Thread.Sleep(0);
            //}

            //return;
        }

        private void SendThread()
        {
            while (!_SerialDevice.Closed)
            {
                if (_DeviceState.Init && _ResponseSignal.Count > 0)
                {
                    try
                    {
                        if (_SerialDevice.CanWrite && !_SerialDevice.Closed)
                        {
                            _SerialDevice.Write(_ResponseSignal.Dequeue());
                            _SerialDevice.PurgeAll();
                        }
                    }
                    catch (Exception exp)
                    {
                        Log.ErrorException(exp.Message, exp);
                        //throw new Exception(exp.Message.ToString());
                    }
                }

                Thread.Sleep(1);
            }
        }

        #endregion

        #region Device Commands

        public void EnableAll()
        {
            _DeviceState.Amount = 0;
            _DeviceState.Nominal = 0;

            var billmask = new BitArray(48);

            if (_BillTable.Count > 0)
            {
                for (int i = 0; i < _BillTable.Count; i++)
                {
                    if (_BillTable[i].Key > 0 && _CurrentCurrency == _BillTable[i].Value)
                    {
                        billmask[(23 - i)] = true;
                    }
                }
            }
            else
            {
                Log.Warn("Bill table is empty!");
            }

            var bytes = ToByteArray(billmask);
            Send(CCNETControllerCommand.EnableDisable, bytes);

            _DeviceState.BillEnable = true;
        }

        public static byte[] ToByteArray(BitArray bits)
        {
            int numBytes = bits.Count / 8;
            if (bits.Count % 8 != 0)
            {
                numBytes++;
            }

            var bytes = new byte[numBytes];
            int byteIndex = 0, bitIndex = 0;

            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                {
                    bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));
                }

                bitIndex++;
                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    byteIndex++;
                }
            }

            return bytes;
        }

        public void Enable(string currency)
        {
            _DeviceState.Amount = 0;
            _DeviceState.Nominal = 0;

            //byte[] billmask = new byte[] { 0xFD, 0x7F, 0x7F };
            //var billmask = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            CurrentCurrency = currency;
            var billmask = new BitArray(48);

            if (_BillTable.Count > 0)
            {
                for (int i = 0; i < _BillTable.Count; i++)
                {
                    if (_BillTable[i].Key > 0 && CurrentCurrency == _BillTable[i].Value)
                    {
                        billmask[(23 - i)] = true;
                    }
                }
            }

            var bytes = ToByteArray(billmask);
            Send(CCNETControllerCommand.EnableDisable, bytes);

            _DeviceState.BillEnable = true;
        }

        public void Disable()
        {
            byte[] disable = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            Send(CCNETControllerCommand.EnableDisable, disable);
            _DeviceState.BillEnable = false;
            _DeviceState.Nominal = 0;
            _DeviceState.Amount = 0;

            Thread.Sleep(100);
        }

        public void Reset()
        {
            _DeviceState.FatalError = false;
            Send(CCNETControllerCommand.Reset, null);
        }

        private void Return()
        {
            // íå ðåàëèçîâàíî
        }

        public void Poll()
        {
            Send(CCNETControllerCommand.Poll, null);
        }

        public void GetBillTable()
        {
            Send(CCNETControllerCommand.GetBillTable, null);
        }

        //private void Hold()
        //{
        //    Send(CCNETControllerCommand.HoldCommand, null);
        //}

        //private void SendNak()
        //{
        //    Send(CCNETControllerCommand.NotMount, null);
        //}

        private void SendAck()
        {
            Send(CCNETControllerCommand.Ok, null);
        }

        #endregion

        #region ParseCommand

        private void ParseCommand(CCNETPacket packet)
        {
            if (packet == null)
            {
                Log.Warn("Packet is null");
                return;
            }
            //int length;
            //int lenCommand = packet.Lng;

            if (packet.Cmd != 0)
            {
                _DeviceState.StateCode = (CCNETResponseStatus)packet.Cmd;
            }

            if (packet.Data != null && packet.Data.Length > 0)
            {
                _DeviceState.SubStateCode = packet.Data[0];
            }

            //Log.Debug(_DeviceState.StateCode.ToString());
            switch (_DeviceState.StateCode)
            {
                case CCNETResponseStatus.Wait:
                    _DeviceState.StateCodeOut = CCNETResponseStatus.Ok;
                    break;

                case CCNETResponseStatus.UnitDisabled:
                    _DeviceState.StateCodeOut = CCNETResponseStatus.UnitDisabled;
                    if (_DeviceState.BillEnable)
                    {
                        EnableAll();
                    }
                    break;

                case CCNETResponseStatus.Accepting:
                    _DeviceState.StateCodeOut = CCNETResponseStatus.Accepting;
                    break;

                case CCNETResponseStatus.Initialize:
                    _DeviceState.StateCodeOut = CCNETResponseStatus.Initialize;
                    break;

                case CCNETResponseStatus.CassetteFull:
                    Log.Warn(CCNETResponseStatus.CassetteFull.ToString());
                    _DeviceState.StateCodeOut = CCNETResponseStatus.CassetteFull;
                    _DeviceState.FatalError = true;
                    _DeviceState.ErrorCode = CCNETErrorCodes.InitialCassetteStatusFailure;
                    //Reset();
                    break;

                case CCNETResponseStatus.CassetteRemoved:
                    _DeviceState.StateCodeOut = CCNETResponseStatus.CassetteRemoved;
                    _DeviceState.WasAmount = _DeviceState.Amount;
                    _DeviceState.Amount = 0;
                    break;

                case CCNETResponseStatus.PowerUpWithBillInAcceptor:
                    _DeviceState.StateCodeOut = CCNETResponseStatus.PowerUpWithBillInAcceptor;
                    Reset();
                    break;

                case CCNETResponseStatus.PowerUpWithBillInStacker:
                    _DeviceState.StateCodeOut = CCNETResponseStatus.PowerUpWithBillInStacker;
                    Reset();
                    break;

                case CCNETResponseStatus.PowerUp:
                    Log.Warn(CCNETResponseStatus.PowerUp.ToString());
                    _DeviceState.StateCodeOut = CCNETResponseStatus.PowerUp;
                    //Reset();
                    break;

                case CCNETResponseStatus.JamInAcceptor:
                    Log.Error(CCNETResponseStatus.JamInAcceptor.ToString());
                    _DeviceState.StateCodeOut = CCNETResponseStatus.JamInAcceptor;
                    _DeviceState.FatalError = true;
                    break;

                case CCNETResponseStatus.Rejecting:
                    Log.Warn(CCNETResponseStatus.Rejecting.ToString());
                    _DeviceState.StateCodeOut = CCNETResponseStatus.Rejecting;
                    _DeviceState.RejectReason = (CCNETRejectReason)_DeviceState.SubStateCode;
                    BillRejected(_DeviceState);
                    break;

                case CCNETResponseStatus.Cheated:
                    Log.Warn(CCNETResponseStatus.Cheated.ToString());
                    _DeviceState.StateCodeOut = CCNETResponseStatus.Cheated;
                    //_DeviceState.FatalError = true;
                    break;

                case CCNETResponseStatus.JamInStacker:
                    Log.Warn(CCNETResponseStatus.JamInStacker.ToString());
                    _DeviceState.StateCodeOut = CCNETResponseStatus.JamInStacker;
                    _DeviceState.FatalError = true;
                    break;

                case CCNETResponseStatus.Error:
                    Log.Warn(CCNETResponseStatus.Error.ToString());
                    _DeviceState.StateCodeOut = CCNETResponseStatus.Error;
                    _DeviceState.ErrorCode = (CCNETErrorCodes)_DeviceState.SubStateCode;
                    //_DeviceState.FatalError = true;
                    break;

                case CCNETResponseStatus.Stacking:
                    _DeviceState.StateCodeOut = CCNETResponseStatus.Stacking;
                    break;

                case CCNETResponseStatus.BillStacked:
                case CCNETResponseStatus.BillAccepting:
                    _DeviceState.Nominal = 0;
                    _DeviceState.StateCodeOut = CCNETResponseStatus.BillAccepting;
                    _DeviceState.Stacking = true;

                    _DeviceState.Currency = CurrentCurrency;

                    KeyValuePair<int, String> value;
                    _BillTable.TryGetValue(_DeviceState.SubStateCode, out value);

                    if (value.Key > 0)
                    {
                        _DeviceState.Stacking = false;
                        _DeviceState.Nominal = value.Key;
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

                case CCNETResponseStatus.BillReturned:
                    Log.Warn(CCNETResponseStatus.BillReturned.ToString());
                    _DeviceState.StateCodeOut = CCNETResponseStatus.BillReturned;
                    break;

                case CCNETResponseStatus.Identification:
                    try
                    {
                        if (packet.Data != null && packet.Data.Length > 30)
                        {
                            var buff = new byte[27];
                            Array.Copy(packet.Data, buff, 27);
                            _DeviceState.Identification = Encoding.ASCII.GetString(buff);
                        }

                        Log.Info(_DeviceState.Identification);
                    }
                    catch (Exception exp)
                    {
                        Log.ErrorException(exp.Message, exp);
                    }
                    break;

                case CCNETResponseStatus.BillTable:
                    try
                    {
                        Log.Debug("CCNETResponseStatus.BillTable");
                        if (packet.Data != null)
                        {
                            Log.Debug(Encoding.ASCII.GetString(packet.Data));
                            _BillTable.Clear();
                            _DeviceState.AvailableCurrencies.Clear();
                            var j = 0;
                            for (int i = 0; (i + 4) < packet.Data.Length; i += 5)
                            {
                                byte first = packet.Data[i];
                                byte second = packet.Data[i + 4];

                                int nominal;
                                if (second > 0)
                                {
                                    nominal = Convert.ToInt32(Math.Round(Math.Pow(10, second), 0));
                                    nominal = nominal * first;
                                }
                                else
                                {
                                    nominal = first;
                                }

                                if (nominal == 0)
                                {
                                    _BillTable.Add(j, new KeyValuePair<int, string>(0, String.Empty));
                                    j++;
                                    continue;
                                }

                                var currency =
                                    Encoding.ASCII.GetString(new[]
                                        {
                                            packet.Data[i + 1], packet.Data[i + 2], packet.Data[i + 3]
                                        });
                                // TODO : Убрать
                                //Log.Debug(currency);
                                currency = GetCurrencyCode(currency);
                                //Log.Debug(currency);
                                if (_DeviceState.AvailableCurrencies.IndexOf(currency) < 0)
                                {
                                    _DeviceState.AvailableCurrencies.Add(currency);
                                }
                                //Log.Debug(String.Format("Nominal: {0}, Currency: {1}", nominal, currency));
                                _BillTable.Add(j, new KeyValuePair<int, string>(nominal, currency));
                                j++;
                            }

                            GetBills(_DeviceState);
                        }
                        else
                        {
                            Log.Warn("Data is null");
                        }
                    }
                    catch (Exception exp)
                    {
                        Log.ErrorException(exp.Message, exp);
                    }
                    break;
            }

            ProccessStateCode();
            ReadCommand(_DeviceState);
        }

        #endregion

        #region Proccess StateCode

        private void ProccessStateCode()
        {
            //switch (_DeviceState.StateCode)
            //{

            //    case CCNETResponseStatus.Inactive:
            //        _DeviceState.DeviceStateDescription = "Íå àêòèâåí";
            //        break;

            //    case CCNETResponseStatus.ReadyForTransaction:
            //        _DeviceState.DeviceStateDescription = "Ãîòîâ ê òðàíçàêöèÿì";
            //        break;

            //    case CCNETResponseStatus.Ok:
            //        _DeviceState.DeviceStateDescription = "ACK";
            //        break;

            //    case CCNETResponseStatus.NotMount:
            //        _DeviceState.DeviceStateDescription = "NAK";
            //        break;

            //    case CCNETResponseStatus.PowerUp:
            //    case CCNETResponseStatus.PowerUpWithBillInAcceptor:
            //    case CCNETResponseStatus.PowerUpWithBillInStacker:
            //        _DeviceState.DeviceStateDescription = EnumEx.GetDescription(CCNETResponseStatus.PowerUp);
            //        break;

            //    default:
            //        _DeviceState.DeviceStateDescription = EnumEx.GetDescription(_DeviceState.StateCode);
            //        break;

            //}


            _DeviceState.DeviceStateDescription = EnumEx.GetDescription(_DeviceState.StateCode);
        }

        #endregion

        #region GetCurrencyCode

        private String GetCurrencyCode(String currency)
        {
            switch (currency)
            {
                case "USA":
                    return Currencies.Usd;

                case "EUR":
                    return Currencies.Eur;

                case "AZE":
                    return Currencies.Azn;
            }

            return String.Empty;
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
                    }
                }

                _Sync = 0x02;
                _Address = address;
                _Lng = Convert.ToByte(dataLength + 6);
                _Cmd = cmd;
                _Data = data;
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
                        // ïîêà íå ðåàëèçîâàíî
                    }
                }

                _Sync = 0x02;
                _Address = address;
                _Lng = Convert.ToByte(dataLength + 6);
                _Cmd = cmd;
                _Data = data;
                _Crc = crc;
            }

            public byte Sync
            {
                get
                {
                    return _Sync;
                }
            }

            public byte Address
            {
                get
                {
                    return _Address;
                }
            }

            public byte Lng
            {
                get
                {
                    return _Lng;
                }
            }

            public byte Cmd
            {
                get
                {
                    return _Cmd;
                }
            }

            public byte[] Data
            {
                get
                {
                    return _Data;
                }
            }

            public ushort Crc
            {
                get
                {
                    return _Crc;
                }
            }
        }

        #endregion

        #region CCNETCRCGenerator

        // ReSharper disable InconsistentNaming
        internal class CCNETCRCGenerator
        // ReSharper restore InconsistentNaming
        {
            const int POLYNOMINAL = 0x08408;
            static private ushort _ByteCrcH;
            static private ushort _ByteCrcL;

            public static ushort GenerateCrc(byte[] dataBuf, ushort bufLen)
            {
                ushort i;
                _ByteCrcH = _ByteCrcL = 0;
                for (i = 0; i < bufLen; i++)
                {
                    CalcCrc(dataBuf[i]);
                }
                i = _ByteCrcH;
                i <<= 8;
                i += _ByteCrcL;
                return i;
            }

            public static byte[] GenerateCrc(byte[] dataBuf, ushort bufLen, bool returnArray)
            {
                ushort i;
                var result = new byte[2];
                _ByteCrcH = _ByteCrcL = 0;
                for (i = 0; i < bufLen; i++)
                {
                    CalcCrc(dataBuf[i]);
                }

                i = _ByteCrcH;
                i <<= 8;
                i += _ByteCrcL;

                result[0] = Convert.ToByte(i >> 8);
                result[1] = Convert.ToByte(_ByteCrcL);
                return result;
            }

            static private void CalcCrc(byte mbyte)
            {
                ushort i;
                _ByteCrcH ^= mbyte;
                ushort tempCrc = _ByteCrcL;
                tempCrc <<= 8;
                tempCrc |= _ByteCrcH;
                for (i = 0; i < 8; i++)
                {
                    var c = (ushort)(tempCrc & 0x01);
                    tempCrc >>= 1;
                    if (c != 0) tempCrc ^= POLYNOMINAL;
                }
                _ByteCrcL = Convert.ToUInt16(tempCrc >> 8);
                _ByteCrcH = Convert.ToUInt16(tempCrc);
            }
        }

        #endregion
    }
}
