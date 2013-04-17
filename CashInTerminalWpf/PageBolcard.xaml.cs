using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CashInTerminal.Enums;
using CashInTerminalWpf.Enums;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageBolcard.xaml
    /// </summary>
    public partial class PageBolcard : Page
    {
        private readonly MainWindow FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageBolcard()
        {
            InitializeComponent();
            FormMain = (MainWindow)Window.GetWindow(this);
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            FormMain.OpenForm(FormEnum.CreditPaymentTypeSelect);
        }

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            if (ClientNumber.Text.Length > 4)
            {
                FormMain.ClientInfo.AccountNumber = ClientNumber.Text;
                FormMain.OpenForm(FormEnum.BolCardRetype);
            }
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Name);
            ControlNumPad.AddHandler(NumPadControl.NewCharEvent, new NumPadControl.NewCharEventHandler(ControlNumPadOnNewChar));
            ControlNumPad.AddHandler(NumPadControl.BackspaceEvent, new NumPadControl.BackspaceEventHandler(ControlNumPadOnBackSpace));
            ControlNumPad.AddHandler(NumPadControl.ClearAllEvent, new NumPadControl.ClearAllEventHandler(ControlNumPadOnClearAll));
        }

        private void ControlNumPadOnNewChar(object sender, NumPadRoutedEventArgs args)
        {
            ClientNumber.Text += args.NewChar;
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

        private void ButtonHomeClick(object sender, RoutedEventArgs e)
        {
            FormMain.OpenForm(FormEnum.Products);
        }
    }
}
