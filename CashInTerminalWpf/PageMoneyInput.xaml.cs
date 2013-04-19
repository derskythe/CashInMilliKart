using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using CashInTerminalWpf.Enums;
using CashInTerminalWpf.Properties;
using Containers.CashCode;
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

        public PageMoneyInput()
        {
            InitializeComponent();           
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Name);
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

            var startCashCodeThread = new Thread(StartCashcode);
            startCashCodeThread.Start();
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            StopCashcode();

            switch ((CheckTemplateTypes)_FormMain.ClientInfo.Product.CheckType)
            {
                case CheckTemplateTypes.CreditPayment:
                    _FormMain.OpenForm(FormEnum.CreditClientInfo);
                    break;

                case CheckTemplateTypes.DebitPayment:
                    _FormMain.OpenForm(FormEnum.DebitClientInfo);
                    break;
            }
        }

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(250);

            if (_FormMain.CcnetDevice.DeviceState.StateCode == CCNETCommand.Stacking)
            {
                return;
            }

            Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(SetNextButton), false);

            Thread.Sleep(250);
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

                    _FormMain.Db.ConfirmTransaction(_FormMain.ClientInfo.PaymentId);
                }

                Log.Debug("Done");
            }
            catch (Exception exp)
            {
                Log.FatalException(exp.Message, exp);
            }

            Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(SetNextButton), true);
            _FormMain.OpenForm(FormEnum.PaySuccess);
        }

        private void CcnetDeviceOnReadCommand(CCNETDeviceState2 ccnetDeviceState)
        {
            //Log.Debug(ccnetDeviceState.ToString());
        }

        private void CcnetDeviceOnBillStacked(CCNETDeviceState2 ccnetDeviceState)
        {
            Log.Info(String.Format("Stacked {0} {1}", ccnetDeviceState.Nominal, ccnetDeviceState.Currency));

            try
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(SetBackButton), false);
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(SetNextButton), true);
                Dispatcher.Invoke(
                    DispatcherPriority.Normal, 
                    new Action<String>(SetStackedAmount), 
                    ccnetDeviceState.Amount.ToString(CultureInfo.InvariantCulture));

                lock (_FormMain.ClientInfo)
                {
                    _FormMain.ClientInfo.CashCodeAmount = ccnetDeviceState.Amount;
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
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(SetNextButton), false);

            try
            {
                Dispatcher.Invoke(
                    DispatcherPriority.Normal,
                    new Action<String>(SetStackedAmount),
                    "0");

                _FormMain.ClientInfo.OrderNumber = 0;
                _FormMain.ClientInfo.PaymentId = _FormMain.Db.InsertTransaction(Convert.ToInt64(_FormMain.ClientInfo.Product.Id),
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
                _FormMain.Db.InsertPaymentValue(_FormMain.ClientInfo.PaymentId, _FormMain.ClientInfo.Client.PassportNumber, 1);

                Log.Info("Starting transId: {0}, PaymentId: {1}", _FormMain.ClientInfo.TransactionId, _FormMain.ClientInfo.PaymentId);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            try
            {
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
