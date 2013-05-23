using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using CashInTerminalWpf.Enums;
using NLog;
using CashInTerminalWpf.CashIn;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageCurrencySelect.xaml
    /// </summary>
    public partial class PageCurrencySelect
    {
        private MainWindow _FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageCurrencySelect()
        {
            InitializeComponent();
        }

        private void ButtonHomeClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.Products);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Title);
            _FormMain = (MainWindow)Window.GetWindow(this);

            try
            {
                if (_FormMain.Currencies != null && _FormMain.Currencies.Count > 0)
                {
                    AddButtons();
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
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

        private void AddButtons()
        {
            lock (_FormMain.Currencies)
            {
                Currency defaultCurrency = null;
                foreach (var currency in _FormMain.Currencies)
                {
                    if (currency.DefaultCurrency)
                    {
                        defaultCurrency = currency;
                        break;
                    }
                }

                Grid.Children.Clear();

                if (defaultCurrency == null)
                {
                    Log.Error("No default currency!");
                    return;
                }

                try
                {
                    foreach (var tag in _FormMain.Currencies)
                    {
                        if ((!tag.DefaultCurrency && defaultCurrency.Id != _FormMain.ClientInfo.Client.Currency && tag.Id != _FormMain.ClientInfo.Client.Currency) || !HasCurrency(tag.Id))
                        {
                            // If default system currency not equal currency of credit
                            // and currency of credit not equal this item currency
                            // or in cashcode we doesn's support this item currency
                            Log.Warn("Can't display currency: " + tag.Id);
                            continue;
                        }

                        string text = tag.Id;
                        Log.Debug(text);

                        var button = new Button
                            {
                                Style = FindResource("MenuButtonStyle") as Style,
                                Name = tag.Id.ToString(CultureInfo.InvariantCulture) + tag.Name,
                                Tag = tag.Id,
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

        private bool HasCurrency(String currency)
        {
            try
            {
                foreach (var availableCurrency in _FormMain.CcnetDevice.DeviceState.AvailableCurrencies)
                {
                    if (currency == availableCurrency)
                    {
                        return true;
                    }
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return false;
        }

        private void ButtonOnClick(object sender, RoutedEventArgs eventArgs)
        {
            try
            {
                var button = (Button)sender;
                Log.Info(button.Name);
                _FormMain.ClientInfo.CurrentCurrency = (String)button.Tag;
                _FormMain.OpenForm(FormEnum.MoneyInput);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }
    }
}
