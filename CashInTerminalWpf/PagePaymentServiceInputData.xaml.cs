using System;
using System.Text.RegularExpressions;
using System.Windows;
using CashInTerminalWpf.CashIn;
using CashInTerminalWpf.CustomControls;
using CashInTerminalWpf.Enums;
using CashInTerminalWpf.Properties;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PagePaymentServiceInputData.xaml
    /// </summary>
    public partial class PagePaymentServiceInputData
    {
        private MainWindow _FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local
        private PaymentServiceField _Field;
        private Regex _Regexp;

        public PagePaymentServiceInputData()
        {
            InitializeComponent();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Title);
            _FormMain = (MainWindow)Window.GetWindow(this);

            if (_FormMain.ClientInfo.PaymentService == null)
            {
                Log.Error("Payment service is null!");
                _FormMain.OpenForm(FormEnum.ServiceNotAvailable);
                return;
            }

            if (_FormMain.ClientInfo.PaymentService.Fields == null)
            {
                Log.Error("No payment fields");
                _FormMain.OpenForm(FormEnum.ServiceNotAvailable);
                return;
            }

            if (_FormMain.ClientInfo.PaymentService.Fields.Length == 0 ||
                _FormMain.ClientInfo.PaymentService.Fields.Length > 1)
            {
                Log.Error("Payment fields is greater than 1 or 0");
                _FormMain.OpenForm(FormEnum.ServiceNotAvailable);
                return;
            }

            _Field = _FormMain.ClientInfo.PaymentService.Fields[0];

            switch (_Field.Type.ToLowerInvariant())
            {
                case PaymentServiceType.Number:
                    ControlAlphabet.Visibility = Visibility.Hidden;
                    ControlNumPad.Visibility = Visibility.Visible;
                    break;

                case PaymentServiceType.Text:
                    ControlAlphabet.Visibility = Visibility.Visible;
                    ControlNumPad.Visibility = Visibility.Visible;
                    break;

                case PaymentServiceType.Select:
                    Log.Error("Field is select!");
                    _FormMain.OpenForm(FormEnum.ServiceNotAvailable);
                    return;
            }


            String text;

            if (!String.IsNullOrEmpty(_Field.LocalizedName.ValueAz))
            {
                text = _Field.LocalizedName.ValueAz;
            }
            else if (!String.IsNullOrEmpty(_Field.LocalizedName.ValueRu))
            {
                text = _Field.LocalizedName.ValueRu;
            }
            else
            {
                text = _Field.LocalizedName.ValueEn;
            }

            LabelCaption.Content = text;
            try
            {
                _Regexp = new Regex(_Field.Regexp, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            }
            catch (Exception exp)
            {
                _Regexp = new Regex("^[a-z0-9]+$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                Log.ErrorException(exp.Message, exp);
            }
            

            ControlNumPad.AddHandler(NumPadControl.NewCharEvent, new NumPadControl.NewCharEventHandler(ControlNumPadOnNewChar));
            ControlNumPad.AddHandler(NumPadControl.BackspaceEvent, new NumPadControl.BackspaceEventHandler(ControlNumPadOnBackSpace));
            ControlNumPad.AddHandler(NumPadControl.ClearAllEvent, new NumPadControl.ClearAllEventHandler(ControlNumPadOnClearAll));
            ControlAlphabet.AddHandler(AlphabetControl.NewCharEvent, new AlphabetControl.NewCharEventHandler(ControlNumPadOnNewChar));
        }

        private void ControlNumPadOnClearAll(object sender, NumPadRoutedEventArgs numPadRoutedEventArgs)
        {
            ClientNumber.Text = String.Empty;
        }

        private void ControlNumPadOnBackSpace(object sender, NumPadRoutedEventArgs args)
        {
            if (ClientNumber.Text.Length > 0)
            {
                ClientNumber.Text = ClientNumber.Text.Substring(0, ClientNumber.Text.Length - 1);
            }
        }

        private void ControlNumPadOnNewChar(object sender, NumPadRoutedEventArgs args)
        {
            ClientNumber.Text += args.NewChar;
        }

        private void ControlNumPadOnNewChar(object sender, AlphabetPadRoutedEventArgs args)
        {
            ClientNumber.Text += args.NewChar;
        }

        private void ButtonHomeClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.Products);
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.PaymentServiceSelect);
        }

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(_Field.NormalizeRegexp))
                {
                    Log.Info("Normalize value");
                    var r = new Regex(_Field.NormalizeRegexp, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                    ClientNumber.Text = r.Replace(ClientNumber.Text, _Field.NormalizePattern);
                    Log.Info(ClientNumber.Text);
                }

                var m = _Regexp.Match(ClientNumber.Text);
                if (m.Success)
                {
                    try
                    {
                        _FormMain.ClientInfo.Client = new CashIn.ClientInfo();
                        _FormMain.ClientInfo.Client.ClientAccount =
                            _FormMain.ClientInfo.Client.ClientCode =
                            _FormMain.ClientInfo.Client.CreditNumber = ClientNumber.Text;
                        var now = DateTime.Now;
                        var info = new PaymentServiceInfo
                            {
                                ServiceId = _FormMain.ClientInfo.PaymentService.Id,
                                ServiceName = _FormMain.ClientInfo.PaymentService.Name,
                                Fields =
                                    new[]
                                        {
                                            new PaymentServiceUserInputField
                                                {
                                                    Name = _Field.Name,
                                                    Value = ClientNumber.Text
                                                }
                                        }
                            };

                        var request = new PaymentServiceInfoRequest
                            {
                                Info = info,
                                SystemTime = now,
                                TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                                Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _FormMain.ServerPublicKey),
                                Ticks = now.Ticks
                            };

                        _FormMain.LongRequestType = LongRequestType.OtherPaymentInfo;
                        _FormMain.InfoRequest = request;
                        _FormMain.OpenForm(FormEnum.Progress);
                    }
                    catch (Exception exp)
                    {
                        Log.ErrorException(exp.Message, exp);
                        _FormMain.OpenForm(FormEnum.ServiceNotAvailable);
                    }
                }
                else
                {
                    Log.Info("Input value doesn't match regex: " + _Field.Regexp);
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }
    }
}
