using System;
using System.Windows;
using CashInTerminalWpf.Enums;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageCreditRequestAccept.xaml
    /// </summary>
    public partial class PageCreditRequestAccept
    {
        private MainWindow _FormMain;
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageCreditRequestAccept()
        {
            InitializeComponent();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Title);
            _FormMain = (MainWindow)Window.GetWindow(this);

            PhoneNumber.Content = _FormMain.ClientInfo.AccountNumber;
        }        

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _FormMain.OpenForm(FormEnum.CreditRequest);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            bool success = false;
            try
            {
                _FormMain.Db.InsertCreditRequest(_FormMain.ClientInfo.AccountNumber);
                success = true;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            try
            {
                _FormMain.OpenForm(success ? FormEnum.CreditRequestSuccess : FormEnum.ServiceNotAvailable);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void ButtonHomeClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _FormMain.OpenForm(FormEnum.Products);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }
    }
}
