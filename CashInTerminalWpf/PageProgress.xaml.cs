using System;
using System.Threading;
using System.Windows;
using CashInTerminalWpf.CashIn;
using CashInTerminalWpf.Enums;
using Containers.Enums;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageProgress.xaml
    /// </summary>
    public partial class PageProgress
    {
        private MainWindow _FormMain;
        private readonly Thread _WorkerThread;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageProgress()
        {
            InitializeComponent();
            _WorkerThread = new Thread(DoWork);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Name);
            _FormMain = (MainWindow)Window.GetWindow(this);            
            _WorkerThread.Start(_FormMain != null ? _FormMain.InfoRequest : null);
        }

        private void DoWork(object value)
        {
            try
            {
                var request = (GetClientInfoRequest)value;

                var response = _FormMain.Server.GetClientInfo(request);
                if (response.ResultCodes != CashIn.ResultCodes.Ok)
                {
                    throw new Exception(response.Description);
                }

                if (response.Infos == null || response.Infos.Length == 0)
                {
                    _FormMain.OpenForm(FormEnum.InvalidNumber);
                    return;
                }

                _FormMain.Clients = response.Infos;

                foreach (var clientInfo in response.Infos)
                {
                    _FormMain.ClientInfo.Client = clientInfo;
                    _FormMain.ClientInfo.CurrentCurrency = clientInfo.Currency;
                    break;
                }

                if (_FormMain.ClientInfo.PaymentOperationType == PaymentOperationType.CreditPaymentByClientCode || _FormMain.ClientInfo.PaymentOperationType == PaymentOperationType.CreditPaymentByPassportAndAccount || _FormMain.ClientInfo.PaymentOperationType == PaymentOperationType.CreditPaymentBolcard)
                {
                    _FormMain.OpenForm(FormEnum.CreditSelectAccount);
                }
                else
                {
                    _FormMain.OpenForm(FormEnum.DebitSelectAccount);
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);

                _FormMain.OpenForm(FormEnum.OutOfOrder);
            }
        }
    }
}
