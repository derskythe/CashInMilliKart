using System;
using System.Globalization;
using System.Windows;
using CashInTerminal.Enums;
using CashInTerminalWpf.CashIn;
using CashInTerminalWpf.Enums;
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
        private Timer _CheckProductTimer;
        private readonly MainWindow _FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageProducts()
        {
            InitializeComponent();

            _FormMain = (MainWindow)Window.GetWindow(this);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Info(Name);

                _FormMain.ProductUpdate += FormMainOnProductUpdate;

                if (_FormMain.Products == null || _FormMain.Products.Count == 0)
                {
                    _FormMain.ForceCheckProducts();
                }

                if (_FormMain.Products != null && _FormMain.Products.Count > 0)
                {
                    AddButtons();
                }

                _CheckProductTimer = new Timer(CheckProductTimer, null, 250, 2000);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void CheckProductTimer(object param)
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
                AddButtons();
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

        private void AddButtons()
        {
            lock (_FormMain.Products)
            {
                Grid.Children.Clear();

                try
                {
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
                            Name = product.Id.ToString(CultureInfo.InvariantCulture) + product.Name,
                            Tag = product,
                            Content = text
                        };
                        button.Click += ButtonOnClick;

                        Grid.Children.Add(button);                        
                    }
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
                _FormMain.OpenForm(product.Assembly);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }        
        
        private void PageUnloaded(object sender, RoutedEventArgs e)
        {
            if (_CheckProductTimer != null)
            {
                _CheckProductTimer.Dispose();
            }
        }
    }
}
