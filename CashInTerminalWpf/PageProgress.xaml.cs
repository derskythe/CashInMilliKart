using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
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
        private readonly DispatcherTimer _Timer;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageProgress()
        {
            InitializeComponent();
            _WorkerThread = new Thread(DoWork);
            _Timer = new DispatcherTimer();
        }

        // ReSharper disable PossibleNullReferenceException
        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Title);
            _FormMain = (MainWindow)Window.GetWindow(this);

            _WorkerThread.Start(_FormMain.InfoRequest);
            if (_FormMain.LongRequestType == LongRequestType.BonusRequest)
            {
                _Timer.Tick += TimerOnTick;
                _Timer.Interval = new TimeSpan(0, 0, 0, 10);
                _Timer.Start();
            }
        }
        // ReSharper restore PossibleNullReferenceException

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            _WorkerThread.Abort();
            _FormMain.OpenForm(FormEnum.PaySuccess);
        }

        private void DoWork(object value)
        {
            try
            {
                if (_FormMain.LongRequestType == LongRequestType.InfoRequest)
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

                    if (_FormMain.ClientInfo.PaymentOperationType == PaymentOperationType.CreditPaymentByClientCode ||
                        _FormMain.ClientInfo.PaymentOperationType ==
                        PaymentOperationType.CreditPaymentByPassportAndAccount ||
                        _FormMain.ClientInfo.PaymentOperationType == PaymentOperationType.CreditPaymentBolcard)
                    {
                        _FormMain.OpenForm(FormEnum.CreditSelectAccount);
                    }
                    else
                    {
                        _FormMain.OpenForm(FormEnum.DebitSelectAccount);
                    }
                }
                else if (_FormMain.LongRequestType == LongRequestType.BonusRequest)
                {
                    try
                    {
                        var request = (BonusRequest)value;

                        var response = _FormMain.Server.GetBonusAmount(request);
                        if (response.ResultCodes != CashIn.ResultCodes.Ok)
                        {
                            throw new Exception(response.Description);
                        }

                        _FormMain.Bonus = response;
                        Log.Info("Client bonus: " + response.Bonus);
                    }
                    catch (Exception exp)
                    {
                        Log.ErrorException(exp.Message, exp);
                    }
                    _FormMain.OpenForm(FormEnum.PaySuccess);
                }
            }
            catch (ThreadAbortException)
            {
                Log.Error("Timeout. Aborting thread");
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);

                _FormMain.OpenForm(FormEnum.OutOfOrder);
            }
        }

        private void PageUnloaded(object sender, RoutedEventArgs e)
        {
            _Timer.Stop();
        }
    }
}
