using System;
using System.Windows;
using CashInTerminalWpf.Enums;
using Containers.Enums;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageCreditClientInfo.xaml
    /// </summary>
    public partial class PageCreditClientInfo
    {
        private MainWindow FormMain;
        private const string DATE_FORMAT = "dd MMMM yyyy";

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageCreditClientInfo()
        {
            InitializeComponent();            
        }

        private void ButtonHomeClick(object sender, RoutedEventArgs e)
        {
            FormMain.OpenForm(FormEnum.Products);
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            FormMain.OpenForm(FormEnum.CreditSelectAccount);
        }

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            FormMain.OpenForm(FormEnum.CurrencySelect);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Title);
            FormMain = (MainWindow)Window.GetWindow(this);

            try
            {
                switch (FormMain.ClientInfo.PaymentOperationType)
                {
                    case PaymentOperationType.CreditPaymentByClientCode:
                        Label1.Content = Properties.Resources.Fullname + Properties.Resources.Colon + Utilities.FirstUpper(FormMain.ClientInfo.Client.FullName);
                        Label2.Content = Properties.Resources.PasportNumber + Properties.Resources.Colon + FormMain.ClientInfo.Client.PassportNumber;
                        Label3.Content = Properties.Resources.AccountNumber + Properties.Resources.Colon + FormMain.ClientInfo.Client.ClientAccount;
                        Label4.Content = Properties.Resources.CreditDate + Properties.Resources.Colon + FormMain.ClientInfo.Client.BeginDate.ToString(DATE_FORMAT);
                        Label5.Content = Properties.Resources.CreditAmount + Properties.Resources.Colon + FormMain.ClientInfo.Client.CreditAmount + @" " + FormMain.ClientInfo.Client.Currency;
                        Label6.Content = Properties.Resources.Currency + Properties.Resources.Colon + FormMain.ClientInfo.Client.Currency;
                        Label7.Content = Properties.Resources.CreditAmountLeft + Properties.Resources.Colon + FormMain.ClientInfo.Client.AmountLeft + @" " + FormMain.ClientInfo.Client.Currency;
                        Label8.Content = Properties.Resources.CreditAmountToPay + Properties.Resources.Colon + FormMain.ClientInfo.Client.AmountLate + @" " + FormMain.ClientInfo.Client.Currency;
                        break;

                    case PaymentOperationType.CreditPaymentByPassportAndAccount:
                        Label1.Content = Properties.Resources.Fullname + Properties.Resources.Colon + Utilities.FirstUpper(FormMain.ClientInfo.Client.FullName);
                        Label2.Content = Properties.Resources.PasportNumber + Properties.Resources.Colon + FormMain.ClientInfo.Client.PassportNumber;
                        Label3.Content = Properties.Resources.AccountNumber + Properties.Resources.Colon + FormMain.ClientInfo.Client.ClientAccount;
                        Label4.Content = Properties.Resources.CreditDate + Properties.Resources.Colon + FormMain.ClientInfo.Client.BeginDate.ToString(DATE_FORMAT);
                        Label5.Content = Properties.Resources.CreditAmount + Properties.Resources.Colon + FormMain.ClientInfo.Client.CreditAmount + @" " + FormMain.ClientInfo.Client.Currency;
                        Label6.Content = Properties.Resources.Currency + Properties.Resources.Colon + FormMain.ClientInfo.Client.Currency;
                        Label7.Content = Properties.Resources.CreditAmountLeft + Properties.Resources.Colon + FormMain.ClientInfo.Client.AmountLeft + @" " + FormMain.ClientInfo.Client.Currency;
                        Label8.Content = Properties.Resources.CreditAmountToPay + Properties.Resources.Colon + FormMain.ClientInfo.Client.AmountLate + @" " + FormMain.ClientInfo.Client.Currency;
                        break;

                    case PaymentOperationType.CreditPaymentBolcard:
                        Label1.Content = Properties.Resources.Fullname + Properties.Resources.Colon + Utilities.FirstUpper(FormMain.ClientInfo.Client.FullName);
                        Label2.Content = Properties.Resources.PasportNumber + Properties.Resources.Colon + FormMain.ClientInfo.Client.PassportNumber;
                        Label3.Content = Properties.Resources.AccountNumber + Properties.Resources.Colon + FormMain.ClientInfo.Client.ClientAccount;
                        Label4.Content = Properties.Resources.CreditDate + Properties.Resources.Colon + FormMain.ClientInfo.Client.BeginDate.ToString(DATE_FORMAT);
                        Label5.Content = Properties.Resources.CreditAmount + Properties.Resources.Colon + FormMain.ClientInfo.Client.CreditAmount + @" " + FormMain.ClientInfo.Client.Currency;
                        Label6.Content = Properties.Resources.Currency + Properties.Resources.Colon + FormMain.ClientInfo.Client.Currency;
                        Label7.Content = Properties.Resources.CreditAmountLeft + Properties.Resources.Colon + FormMain.ClientInfo.Client.AmountLeft + @" " + FormMain.ClientInfo.Client.Currency;
                        Label8.Content = Properties.Resources.CreditAmountToPay + Properties.Resources.Colon + FormMain.ClientInfo.Client.AmountLate + @" " + FormMain.ClientInfo.Client.Currency;
                        break;
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }
    }
}
