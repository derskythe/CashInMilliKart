using System;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;
using CashInTerminalWpf.CashIn;
using CashInTerminalWpf.Enums;
using CashInTerminalWpf.Properties;
using NLog;
using Button = System.Windows.Controls.Button;
using Timer = System.Threading.Timer;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageProducts.xaml
    /// </summary>
    public partial class PageProducts
    {
        private DispatcherTimer _CheckProductTimer;
        private MainWindow _FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageProducts()
        {
            InitializeComponent();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Info(Title);
                _FormMain = (MainWindow)Window.GetWindow(this);

                LabelTestVersion.Visibility = App.TestVersion ? Visibility.Visible : Visibility.Collapsed;
                LabelTerminal.Content = Settings.Default.TerminalCode;

                _FormMain.ProductUpdate += FormMainOnProductUpdate;

                if (_FormMain.Products == null || _FormMain.Products.Count == 0)
                {
                    _FormMain.ForceCheckProducts();
                }

                if (_FormMain.Products != null && _FormMain.Products.Count > 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(AddButtons));
                }

                _CheckProductTimer = new DispatcherTimer();
                _CheckProductTimer.Tick += CheckProductTimer;
                _CheckProductTimer.Interval = new TimeSpan(0, 0, 60);
                _CheckProductTimer.Start();
                //_CheckProductTimer = new Timer(CheckProductTimer, null, 250, 2000);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void CheckProductTimer(object sender, EventArgs e)
        {
            try
            {
                if (Grid.Children.Count == 0)
                {
                    _FormMain.OpenForm(FormEnum.OutOfOrder);
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }        

        private void FormMainOnProductUpdate()
        {
            try
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(AddButtons));
                if (_FormMain.Products.Count == 0)
                {
                    _FormMain.OpenForm(FormEnum.OutOfOrder);
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void AddButtons()
        {
            lock (_FormMain.Products)
            {
                try
                {
                    Grid.Children.Clear();

                    foreach (var product in _FormMain.Products)
                    {
                        string text;

                        if (!String.IsNullOrEmpty(product.NameAz))
                        {
                            text = product.NameAz;
                        }
                        else if (!String.IsNullOrEmpty(product.NameEn))
                        {
                            text = product.NameEn;
                        }
                        else
                        {
                            text = product.NameRu;
                        }
                        Log.Debug(text);

                        var button = new Button
                        {
                            Style = FindResource("MenuButtonStyle") as Style,
                            Name = product.Name + product.Id.ToString(CultureInfo.InvariantCulture),
                            Tag = product,
                            Content = text
                        };
                        button.Click += ButtonOnClick;

                        Grid.Children.Add(button);
                    }

                    //
                    // TODO : DELETE IN PRODUCTION
                    //
                    var virtualProduct = new Product
                        {
                            Assembly = "PageCreditRequest.xaml",
                            Id = 9999,
                            Name = "CreditRequest"
                        };
                    virtualProduct.NameAz = virtualProduct.NameEn = virtualProduct.NameRu = "Credit Request";

                    var virtualButton = new Button
                    {
                        Style = FindResource("MenuButtonStyle") as Style,
                        Name = virtualProduct.Name + virtualProduct.Id.ToString(CultureInfo.InvariantCulture),
                        Tag = virtualProduct,
                        Content = virtualProduct.NameAz
                    };
                    virtualButton.Click += ButtonOnClick;

                    Grid.Children.Add(virtualButton);
                    //
                    //
                    //
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                }
            }
        }

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            try
            {
                var button = (Button)sender;
                Log.Info(String.Format("Selected: {0}", button.Name));
                var product = (Product)button.Tag;

                _FormMain.ClientInfo.Product = product;
                String url;
                switch (product.Assembly)
                {
                    case "FormCreditTypeSelect":
                        url = FormEnum.CreditPaymentTypeSelect;
                        break;

                    case "FormDebitPayType":
                        url = FormEnum.DebitPaymentTypeSelect;
                        break;

                    default:
                        url = product.Assembly;
                        break;
                }
                _FormMain.OpenForm(url);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void PageUnloaded(object sender, RoutedEventArgs e)
        {
            _CheckProductTimer.Stop();
            _CheckProductTimer.IsEnabled = false;
        }
    }
}
