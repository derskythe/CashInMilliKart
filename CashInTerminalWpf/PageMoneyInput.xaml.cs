using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using CashInTerminalWpf.Enums;
using CashInTerminalWpf.Properties;
using Containers;
using Containers.CashCode;
using Containers.Enums;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageMoneyInput.xaml
    /// </summary>
    public partial class PageMoneyInput
    {
        private MainWindow _FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local
        private readonly TimeSpan _MaxTransactionTime = new TimeSpan(0, 0, 0, 30);
        private CCNETResponseStatus _LastResponse = CCNETResponseStatus.NotMount;
        private DateTime _LastInput = DateTime.Now;
        private int _Amount;

        public PageMoneyInput()
        {
            InitializeComponent();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Title);
            _Amount = 0;
            _FormMain = (MainWindow)Window.GetWindow(this);

            if (_FormMain.ClientInfo.CurrentCurrency != _FormMain.ClientInfo.Client.Currency)
            {
                LabelCommission.Visibility = Visibility.Visible;
            }

            LabelCurrency.Content = _FormMain.ClientInfo.CurrentCurrency;
            LabelTotal.Content = @"0";
            _FormMain.ClientInfo.CashCodeAmount = 0;

            _FormMain.CcnetDevice.BillStacked += CcnetDeviceOnBillStacked;
            _FormMain.CcnetDevice.ReadCommand += CcnetDeviceOnReadCommand;
            //_FormMain.CcnetDevice.BillRejected += CcnetDeviceOnBillRejected;

            var startCashCodeThread = new Thread(StartCashcode);
            startCashCodeThread.Start();
        }

        //private void CcnetDeviceOnBillRejected(CCNETDeviceState ccnetDeviceState)
        //{
        //    Log.Info("Bill reject reason: " + ccnetDeviceState.RejectReason + " " + EnumEx.GetDescription(ccnetDeviceState.RejectReason));
        //}

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            StopCashcode();

            switch (_FormMain.ClientInfo.PaymentOperationType)
            {
                case PaymentOperationType.CreditPaymentByClientCode:
                case PaymentOperationType.CreditPaymentByPassportAndAccount:
                case PaymentOperationType.CreditPaymentBolcard:
                    _FormMain.OpenForm(FormEnum.CreditClientInfo);
                    break;

                case PaymentOperationType.DebitPaymentByClientCode:
                case PaymentOperationType.DebitPaymentByPassportAndAccount:
                    _FormMain.OpenForm(FormEnum.DebitClientInfo);
                    break;

                case PaymentOperationType.Komtek:
                case PaymentOperationType.GoldenPay:
                    _FormMain.OpenForm(FormEnum.OtherPaymentUserInfo);
                    break;

                default:
                    _FormMain.OpenForm(FormEnum.Products);
                    break;
            }
        }

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            ButtonNext.IsEnabled = false;
            EndTransaction();
        }

        private void EndTransaction()
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(SetNextButton), false);

            try
            {
                Log.Debug("Stopping cashcode");
                StopCashcode();
                Log.Debug("Stoped cashcode");
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            try
            {
                Log.Debug("Update DB");

                lock (_FormMain.ClientInfo)
                {
                    _FormMain.Db.UpdateAmount(
                        _FormMain.ClientInfo.PaymentId,
                        _FormMain.ClientInfo.CurrentCurrency,
                        1,
                        Convert.ToInt32(_FormMain.ClientInfo.CashCodeAmount));
                    // TODO : REMOVE THIS!!!!!!!!!!!!!!!!!!
                    _FormMain.Db.ConfirmTransaction(_FormMain.ClientInfo.PaymentId);
                }

                Log.Debug("Done");
            }
            catch (Exception exp)
            {
                Log.FatalException(exp.Message, exp);
            }

            Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(SetNextButton), true);

            if (_FormMain.ClientInfo.PaymentOperationType == PaymentOperationType.CreditPaymentByClientCode ||
                _FormMain.ClientInfo.PaymentOperationType ==
                PaymentOperationType.CreditPaymentByPassportAndAccount ||
                _FormMain.ClientInfo.PaymentOperationType == PaymentOperationType.CreditPaymentBolcard)
            {
                var now = DateTime.Now;
                var request = new CashIn.BonusRequest
                {
                    CreditNumber = _FormMain.ClientInfo.Client.CreditNumber,
                    Amount = _FormMain.ClientInfo.CashCodeAmount,
                    Currency = _FormMain.ClientInfo.CurrentCurrency,
                    SystemTime = now,
                    TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                    Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _FormMain.ServerPublicKey)
                };

                _FormMain.InfoResponse = null;
                _FormMain.LongRequestType = LongRequestType.Bonus;
                _FormMain.InfoRequest = request;

                _FormMain.OpenForm(FormEnum.Progress);
            }
            else
            {
                _FormMain.OpenForm(FormEnum.PaySuccess);
            }
        }

        private void CcnetDeviceOnReadCommand(CCNETDeviceState ccnetDeviceState)
        {
            Log.Debug(String.Format("State: {0}, SubState: {1}, ErrorCode: {2}", ccnetDeviceState.StateCodeOut, ccnetDeviceState.SubStateCode, ccnetDeviceState.ErrorCode));

            if (ccnetDeviceState.StateCode != CCNETResponseStatus.Idling && ccnetDeviceState.StateCode != CCNETResponseStatus.Wait)
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(SetNextButton), false);

                if (ccnetDeviceState.StateCode == CCNETResponseStatus.Accepting ||
                    ccnetDeviceState.StateCode == CCNETResponseStatus.BillAccepting ||
                    ccnetDeviceState.StateCode == CCNETResponseStatus.BillReceiving ||
                    ccnetDeviceState.StateCode == CCNETResponseStatus.BillStacked ||
                    ccnetDeviceState.StateCode == CCNETResponseStatus.Stacking
                    )
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(SetBackButton), false);
                }
            }
            else if ((ccnetDeviceState.StateCode == CCNETResponseStatus.Idling || ccnetDeviceState.StateCode == CCNETResponseStatus.Wait) && _FormMain.ClientInfo.CashCodeAmount > 0)
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(SetNextButton), true);
            }

            if (_LastResponse != ccnetDeviceState.StateCodeOut)
            {
                _LastResponse = ccnetDeviceState.StateCodeOut;
                _LastInput = DateTime.Now;
            }
            else if (DateTime.Now - _LastInput > _MaxTransactionTime)
            {
                Log.Warn("Autocommit transaction");
                if (_FormMain.ClientInfo.CashCodeAmount > 0)
                {
                    Log.Info("Transaction commit");
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(EndTransaction));
                }
                else
                {
                    Log.Info("Transaction rollback");
                    StopCashcode();
                    _FormMain.OpenForm(FormEnum.Products);
                }
            }
        }

        private void CcnetDeviceOnBillStacked(CCNETDeviceState ccnetDeviceState)
        {
            Log.Info(String.Format("Stacked {0} {1}", ccnetDeviceState.Nominal, ccnetDeviceState.Currency));

            try
            {
                //Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(SetBackButton), false);
                //Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(SetNextButton), true);
                Dispatcher.Invoke(
                    DispatcherPriority.Normal,
                    new Action<String>(SetStackedAmount),
                    ccnetDeviceState.Amount.ToString(CultureInfo.InvariantCulture));

                _Amount += ccnetDeviceState.Nominal;
                lock (_FormMain.ClientInfo)
                {
                    _FormMain.ClientInfo.CashCodeAmount = _Amount;
                }

                _FormMain.Db.InsertBanknote(
                                            _FormMain.ClientInfo.PaymentId,
                                            ccnetDeviceState.Nominal,
                                            ccnetDeviceState.Currency,
                                            _FormMain.ClientInfo.OrderNumber++);
                _FormMain.Db.InsertTransactionBanknotes(
                    ccnetDeviceState.Nominal,
                    ccnetDeviceState.Currency,
                   _FormMain.ClientInfo.TransactionId);
            }
            catch (Exception exp)
            {
                Log.FatalException(exp.Message, exp);
            }
        }

        private void PageUnloaded(object sender, RoutedEventArgs e)
        {
            _FormMain.CcnetDevice.BillStacked -= CcnetDeviceOnBillStacked;
            _FormMain.CcnetDevice.ReadCommand -= CcnetDeviceOnReadCommand;
        }

        private void StartCashcode()
        {           
            try
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(SetNextButton), false);

                Dispatcher.Invoke(
                    DispatcherPriority.Normal,
                    new Action<String>(SetStackedAmount),
                    "0");

                _FormMain.ClientInfo.OrderNumber = 0;
                int serviceId = 0;

                if (_FormMain.ClientInfo.PaymentService != null)
                {
                    serviceId = _FormMain.ClientInfo.PaymentService.Id;
                }
                _FormMain.ClientInfo.PaymentId = _FormMain.Db.InsertTransaction(Convert.ToInt64(_FormMain.ClientInfo.Product.Id),
                    serviceId,
                                                                              _FormMain.ClientInfo.CurrentCurrency,
                                                                              1,
                                                                              0,
                                                                              Convert.ToInt32(
                                                                                  Settings.Default.TerminalCode),
                                                                              _FormMain.ClientInfo.Client.CreditNumber,
                                                                              (int)_FormMain.ClientInfo.PaymentOperationType,
                                                                              false);
                int termCode = Convert.ToInt32(Settings.Default.TerminalCode);
                _FormMain.ClientInfo.TransactionId = String.Format("{0}{1}{2}", termCode.ToString("000"),
                                               DateTime.Now.DayOfYear.ToString("000"), _FormMain.ClientInfo.PaymentId);

                _FormMain.Db.UpdateTransactionId(_FormMain.ClientInfo.PaymentId, _FormMain.ClientInfo.TransactionId);
                _FormMain.Db.InsertPaymentValue(_FormMain.ClientInfo.PaymentId, _FormMain.ClientInfo.Client.ClientAccount, 0);
                if (!String.IsNullOrEmpty(_FormMain.ClientInfo.Client.PassportNumber))
                {
                    _FormMain.Db.InsertPaymentValue(_FormMain.ClientInfo.PaymentId, _FormMain.ClientInfo.Client.PassportNumber, 1);
                }

                Log.Info("Starting transId: {0}, PaymentId: {1}", _FormMain.ClientInfo.TransactionId, _FormMain.ClientInfo.PaymentId);

                _FormMain.CcnetDevice.Poll();
                _FormMain.CcnetDevice.Enable(_FormMain.ClientInfo.CurrentCurrency.ToLower());
                _FormMain.CcnetDevice.StartPool = true;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }            
        }

        private void StopCashcode()
        {
            _FormMain.CcnetDevice.StartPool = false;
            _FormMain.CcnetDevice.Disable();
        }

        private void SetStackedAmount(String amount)
        {
            try
            {
                LabelTotal.Content = amount;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void SetNextButton(bool status)
        {
            try
            {
                ButtonNext.IsEnabled = status;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void SetBackButton(bool status)
        {
            try
            {
                ButtonBack.IsEnabled = status;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }
    }
}
