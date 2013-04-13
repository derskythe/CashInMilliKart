using System;
using System.Linq;
using System.Threading;
using CashControlTerminal;
using CashInTerminal.Enums;
using Containers;
using Containers.CashCode;
using NLog;

namespace CashInTerminal
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
        private const int MAX_RESPONSE_TIME = 250;
        /*
                private const int MAX_PACKET_LENGTH = 255;
        */

        private readonly Thread _ResponseReaderThread;
        private readonly Thread _PoolThread;
        //private readonly Thread _SendThread;
        //private Thread _EventThread;
        private readonly CCNETDeviceState _DeviceState;
        private String _CurrentCurrency;
        private int _Port;
        private CCNETPortSpeed _PortSpeed;
        private readonly Object _ResponseSignal;
        //private CCNETPacket _ResponsePacket;
        private readonly EventWaitHandle _Wait = new AutoResetEvent(false);
        private readonly Timer _BackgroundPingTimer;
        private bool _TimedOut;
        private bool _Disposing;

        public delegate void ReadCommandHandler(CCNETDeviceState e);
        public delegate void BillStackedHandler(CCNETDeviceState e);
        public event ReadCommandHandler ReadCommand = delegate { };
        public event BillStackedHandler BillStacked = delegate { };

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
            _ResponseSignal = new Object();

            // Create the Thread used to read responses
            _ResponseReaderThread = new Thread(ReadResponseThread) { IsBackground = true };

            // Create the Thread used to send POOL command
            _PoolThread = new Thread(StartPooling) { IsBackground = true };

            _SerialDevice = new SerialStream();
            _BackgroundPingTimer = new Timer(BackgroundPingTimer, null, 0, 60*1000);
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

            _BackgroundPingTimer.Dispose();

            if (_ResponseReaderThread != null && _ResponseReaderThread.IsAlive)
            {
                _ResponseReaderThread.Join(5000);
            }

            if (_PoolThread != null && _PoolThread.IsAlive)
            {
                _PoolThread.Join(5000);
            }

            //if (_SendThread != null && _SendThread.IsAlive)
            //{
            //    _SendThread.Join(5000);
            //}

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
                byte[] sec = { 0x00, 0x00, 0x00 };
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
                    _DeviceState.StateCode = CCNETCommand.FatalError;
                    ProccessStateCode();
                }
            }
            catch (ThreadAbortException exp)
            {
                Log.Error(exp.Message);
                //Thread.CurrentThread.Abort();
            }
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
                    Send(CCNETCommand.Poll, null);
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
                Send(CCNETCommand.Poll, null);
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

            crc |= buffer[(startIndex + 4 + dataSection) % buffer.Length];
            crc |= (ushort)(buffer[(startIndex + 4 + dataSection + 1) % buffer.Length] << 8);
            return new CCNETPacket(addr, cmd, data, crc);
        }

        private void AddPacket(byte[] buffer, int startIndex)
        {
            if (buffer == null)
            {
                return;
            }

            CCNETPacket packet = CreatePacket(buffer, startIndex);

            if (packet.Cmd != (byte)CCNETCommand.Ok || packet.Cmd != (byte)CCNETCommand.NotMount)
            {
                SendAck();
                ParseCommand(packet);
            }

            AddResponsePacket();
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
            ushort generateCrc = CCNETCRCGenerator.GenerateCrc(crc, Convert.ToUInt16(crc.Length));


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
            packetXMitBuffer[packet.Lng - 2] = (byte)(generateCrc >> 8);
            packetXMitBuffer[packet.Lng - 1] = (byte)(generateCrc);

            lock (_ResponseSignal)
            {
                //_ResponsePacket = null;
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
                    Log.ErrorException(exp.Message, exp);
                    //throw new Exception(exp.Message.ToString());
                }

                Monitor.Wait(_ResponseSignal, MAX_RESPONSE_TIME);
                Thread.Sleep(0);
            }
        }

        #endregion

        #region Device Commands

        public void EnableAll()
        {
            _DeviceState.Amount = 0;
            _DeviceState.Nominal = 0;

            //byte[] billmask = new byte[] { 0xFD, 0x7F, 0x7F };
            var billmask = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            //if (_CurrentCurrency != null)
            //{
            //    foreach (var item in _CurrentCurrency)
            //    {
            switch (_CurrentCurrency)
            {
                case Currencies.Azn:
                    billmask[0] = 0x7D;
                    break;

                case Currencies.Eur:
                    billmask[1] = 0x7F;
                    break;

                case Currencies.Usd:
                    billmask[2] = 0x7F;
                    break;
            }
            //    }
            //}
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

        public void Enable(string currency)
        {
            _DeviceState.Amount = 0;
            _DeviceState.Nominal = 0;

            //byte[] billmask = new byte[] { 0xFD, 0x7F, 0x7F };
            var billmask = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            CurrentCurrency = currency;
            //if (_CurrentCurrency == null)
            //{
            //    _CurrentCurrency = new List<string>();
            //}
            //else
            //{
            //    _CurrentCurrency.Clear();
            //}
            //_CurrentCurrency.Add(curr);

            switch (CurrentCurrency)
            {
                case Currencies.Azn:
                    billmask[0] = 0x7D;
                    break;

                case Currencies.Eur:
                    billmask[1] = 0x7F;
                    break;

                case Currencies.Usd:
                    billmask[2] = 0x7F;
                    break;
            }
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
            byte[] disable = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            Send(CCNETCommand.EnableDisable, disable);
            _DeviceState.BillEnable = false;
            _DeviceState.Nominal = 0;
            _DeviceState.Amount = 0;

            Thread.Sleep(100);
        }

        public void Reset()
        {
            Send(CCNETCommand.Reset, null);
        }

        private void Return()
        {
            // не реализовано
        }

        public void Poll()
        {
            Send(CCNETCommand.Poll, null);
        }

        //private void Hold()
        //{
        //    Send(CCNETCommand.Hold, null);
        //}

        //private void SendNAK()
        //{
        //    Send(CCNETCommand.NotMount, null);
        //}

        private void SendAck()
        {
            Send(CCNETCommand.Ok, null);
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
                    Log.Warn(CCNETCommand.StackerFull.ToString());
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
                    Log.Warn(CCNETCommand.PowerUp.ToString());
                    _DeviceState.StateCodeOut = CCNETCommand.PowerUp;
                    //Reset();
                    break;

                case CCNETCommand.BillJam:
                    Log.Error(CCNETCommand.BillJam.ToString());
                    _DeviceState.StateCodeOut = CCNETCommand.BillJam;
                    _DeviceState.FatalError = true;
                    break;

                case CCNETCommand.FatalError:
                    Log.Warn(CCNETCommand.FatalError.ToString());
                    _DeviceState.StateCodeOut = CCNETCommand.FatalError;
                    _DeviceState.FatalError = true;
                    break;

                case CCNETCommand.Cheated:
                    Log.Warn(CCNETCommand.Cheated.ToString());
                    _DeviceState.StateCodeOut = CCNETCommand.Cheated;
                    _DeviceState.FatalError = true;
                    break;

                case CCNETCommand.CasseteBillJam:
                    Log.Warn(CCNETCommand.CasseteBillJam.ToString());
                    _DeviceState.StateCodeOut = CCNETCommand.CasseteBillJam;
                    _DeviceState.FatalError = true;
                    break;

                case CCNETCommand.Error:
                    Log.Warn(CCNETCommand.Error.ToString());
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

                    _DeviceState.Currency = CurrentCurrency;
                    switch (_DeviceState.SubStateCode)
                    {
                        case CcnetBillTypes.Azn1: // 1 AZN
                            _DeviceState.Nominal = 1;

                            break;

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

                        case CcnetBillTypes.Usd1:
                            _DeviceState.Nominal = 1;
                            break;

                        case CcnetBillTypes.Usd2:
                            _DeviceState.Nominal = 2;
                            break;

                        case CcnetBillTypes.Usd5:
                            _DeviceState.Nominal = 5;
                            break;

                        case CcnetBillTypes.Usd10:
                            _DeviceState.Nominal = 10;
                            break;

                        case CcnetBillTypes.Usd20:
                            _DeviceState.Nominal = 20;
                            break;

                        case CcnetBillTypes.Usd50:
                            _DeviceState.Nominal = 50;
                            break;

                        case CcnetBillTypes.Usd100:
                            _DeviceState.Nominal = 100;
                            break;

                        case CcnetBillTypes.Eur5:
                            _DeviceState.Nominal = 5;
                            break;

                        case CcnetBillTypes.Eur10:
                            _DeviceState.Nominal = 10;
                            break;

                        case CcnetBillTypes.Eur20:
                            _DeviceState.Nominal = 20;
                            break;

                        case CcnetBillTypes.Eur50:
                            _DeviceState.Nominal = 50;
                            break;

                        case CcnetBillTypes.Eur100:
                            _DeviceState.Nominal = 100;
                            break;

                        case CcnetBillTypes.Eur200:
                            _DeviceState.Nominal = 200;
                            break;

                        case CcnetBillTypes.Eur500:
                            _DeviceState.Nominal = 500;
                            break;
                    }

                    //foreach (var currency in _CurrentCurrency)
                    //{
                    //    if (currency == Currencies.Azn)
                    //    {
                    //        _DeviceState.Currency = Currencies.Azn;
                    //        switch (_DeviceState.SubStateCode)
                    //        {
                    //            case CcnetBillTypes.Azn1: // 1 AZN
                    //                _DeviceState.Nominal = 1;

                    //                break;

                    //            case CcnetBillTypes.Azn5: // 5 AZN
                    //                _DeviceState.Nominal = 5;
                    //                break;

                    //            case CcnetBillTypes.Azn10: // 10 AZN
                    //                _DeviceState.Nominal = 10;
                    //                break;

                    //            case CcnetBillTypes.Azn20: // 20 AZN
                    //                _DeviceState.Nominal = 20;
                    //                break;

                    //            case CcnetBillTypes.Azn50: // 50 AZN
                    //                _DeviceState.Nominal = 50;
                    //                break;

                    //            case CcnetBillTypes.Azn100:
                    //                _DeviceState.Nominal = 100;
                    //                break;
                    //        }
                    //    }
                    //    else if (currency == Currencies.Usd)
                    //    {
                    //        _DeviceState.Currency = Currencies.Usd;
                    //        switch (_DeviceState.SubStateCode)
                    //        {
                    //            case CcnetBillTypes.Usd1:
                    //                _DeviceState.Nominal = 1;
                    //                break;

                    //            case CcnetBillTypes.Usd2:
                    //                _DeviceState.Nominal = 2;
                    //                break;

                    //            case CcnetBillTypes.Usd5:
                    //                _DeviceState.Nominal = 5;
                    //                break;

                    //            case CcnetBillTypes.Usd10:
                    //                _DeviceState.Nominal = 10;
                    //                break;

                    //            case CcnetBillTypes.Usd20:
                    //                _DeviceState.Nominal = 20;
                    //                break;

                    //            case CcnetBillTypes.Usd50:
                    //                _DeviceState.Nominal = 50;
                    //                break;

                    //            case CcnetBillTypes.Usd100:
                    //                _DeviceState.Nominal = 100;
                    //                break;
                    //        }
                    //    }
                    //    else if (currency == Currencies.Eur)
                    //    {
                    //        _DeviceState.Currency = Currencies.Eur;
                    //        switch (_DeviceState.SubStateCode)
                    //        {
                    //            case CcnetBillTypes.Eur5:
                    //                _DeviceState.Nominal = 5;
                    //                break;

                    //            case CcnetBillTypes.Eur10:
                    //                _DeviceState.Nominal = 10;
                    //                break;

                    //            case CcnetBillTypes.Eur20:
                    //                _DeviceState.Nominal = 20;
                    //                break;

                    //            case CcnetBillTypes.Eur50:
                    //                _DeviceState.Nominal = 50;
                    //                break;

                    //            case CcnetBillTypes.Eur100:
                    //                _DeviceState.Nominal = 100;
                    //                break;

                    //            case CcnetBillTypes.Eur200:
                    //                _DeviceState.Nominal = 200;
                    //                break;

                    //            case CcnetBillTypes.Eur500:
                    //                _DeviceState.Nominal = 500;
                    //                break;
                    //        }
                    //    }
                    //}


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
                    Log.Warn(CCNETCommand.BillReturned.ToString());
                    _DeviceState.StateCodeOut = CCNETCommand.BillReturned;
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

            //    case CCNETCommand.Inactive:
            //        _DeviceState.DeviceStateDescription = "Не активен";
            //        break;

            //    case CCNETCommand.ReadyForTransaction:
            //        _DeviceState.DeviceStateDescription = "Готов к транзакциям";
            //        break;

            //    case CCNETCommand.Ok:
            //        _DeviceState.DeviceStateDescription = "ACK";
            //        break;

            //    case CCNETCommand.NotMount:
            //        _DeviceState.DeviceStateDescription = "NAK";
            //        break;

            //    case CCNETCommand.PowerUp:
            //    case CCNETCommand.PowerUpWithBillInAcceptor:
            //    case CCNETCommand.PowerUpWithBillInStacker:
            //        _DeviceState.DeviceStateDescription = EnumEx.GetDescription(CCNETCommand.PowerUp);
            //        break;

            //    default:
            //        _DeviceState.DeviceStateDescription = EnumEx.GetDescription(_DeviceState.StateCode);
            //        break;

            //}
            
            _DeviceState.DeviceStateDescription = EnumEx.GetDescription(_DeviceState.StateCode);
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
                        // пока не реализовано
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
                byte[] result = new byte[2];
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
                ushort i, c;
                ushort temp_crc;
                _ByteCrcH ^= mbyte;
                temp_crc = _ByteCrcL;
                temp_crc <<= 8;
                temp_crc |= _ByteCrcH;
                for (i = 0; i < 8; i++)
                {
                    c = (ushort)(temp_crc & 0x01);
                    temp_crc >>= 1;
                    if (c != 0) temp_crc ^= POLYNOMINAL;
                }
                _ByteCrcL = Convert.ToUInt16(temp_crc >> 8);
                _ByteCrcH = Convert.ToUInt16(temp_crc);
            }
        }

        #endregion

    }
}
