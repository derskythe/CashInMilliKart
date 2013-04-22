using System.Windows;
using CashInTerminalWpf.Enums;
using Containers.Enums;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageDebitPaymentTypeSelect.xaml
    /// </summary>
    public partial class PageDebitPaymentTypeSelect
    {
        private MainWindow FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageDebitPaymentTypeSelect()
        {
            InitializeComponent();            
        }

        private void ButtonClientNumberClick(object sender, RoutedEventArgs e)
        {
            FormMain.ClientInfo.PaymentOperationType = PaymentOperationType.DebitPaymentByClientCode;
            FormMain.OpenForm(FormEnum.ClientCode);
        }

        private void ButtonCreditNumberAndPasportClick(object sender, RoutedEventArgs e)
        {
            FormMain.ClientInfo.PaymentOperationType = PaymentOperationType.DebitPaymentByPassportAndAccount;
            FormMain.OpenForm(FormEnum.ClientByPassport);
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            FormMain.OpenForm(FormEnum.Products);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Title);
            LabelTestVersion.Visibility = App.TestVersion ? Visibility.Visible : Visibility.Collapsed;

            FormMain = (MainWindow)Window.GetWindow(this);
        }
    }
}
