using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using CashInTerminalWpf.CashIn;
using CashInTerminalWpf.Enums;
using Containers.Enums;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageOtherPaymentsCategories.xaml
    /// </summary>
    public partial class PageOtherPaymentsCategories
    {
        private MainWindow _FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageOtherPaymentsCategories()
        {
            InitializeComponent();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Title);
            _FormMain = (MainWindow)Window.GetWindow(this);

            try
            {
                AddButtons();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
                _FormMain.OpenForm(FormEnum.OutOfOrder);
            }
        }

        private void AddButtons()
        {
            lock (_FormMain.PaymentCategories)
            {
                try
                {
                    Grid.Children.Clear();

                    foreach (var category in _FormMain.PaymentCategories)
                    {
                        string text;
                        if (!String.IsNullOrEmpty(category.LocalizedName.ValueAz))
                        {
                            text = category.LocalizedName.ValueAz;
                        }
                        else if (!String.IsNullOrEmpty(category.LocalizedName.ValueEn))
                        {
                            text = category.LocalizedName.ValueEn;
                        }
                        else
                        {
                            text = category.LocalizedName.ValueRu;
                        }
                        Log.Debug(text);

                        var button = new Button
                        {
                            Style = FindResource("MenuButtonStyle") as Style,
                            Name = category.Name + category.Id.ToString(CultureInfo.InvariantCulture),
                            Tag = category,
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
                var tag = (PaymentCategory)button.Tag;
                
                _FormMain.ClientInfo.PaymentCategory = tag;
                _FormMain.OpenForm(FormEnum.PaymentServiceSelect);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.Products);
        }
    }
}
