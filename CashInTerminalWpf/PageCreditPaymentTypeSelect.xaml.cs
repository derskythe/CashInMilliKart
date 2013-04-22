using System.Windows;
using System.Windows.Controls;
using CashInTerminalWpf.Enums;
using Containers.Enums;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageCreditPaymentTypeSelect.xaml
    /// </summary>
    public partial class PageCreditPaymentTypeSelect
    {
        private MainWindow _FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageCreditPaymentTypeSelect()
        {
            InitializeComponent();            
        }

        private void ButtonClientNumberClick(object sender, RoutedEventArgs e)
        {
            _FormMain.ClientInfo.PaymentOperationType = PaymentOperationType.CreditPaymentByClientCode;
            _FormMain.OpenForm(FormEnum.ClientCode);
        }

        private void ButtonCreditNumberAndPasportClick(object sender, RoutedEventArgs e)
        {
            _FormMain.ClientInfo.PaymentOperationType = PaymentOperationType.CreditPaymentByPassportAndAccount;
            _FormMain.OpenForm(FormEnum.ClientByPassport);
        }

        private void ButtonBolCardClick(object sender, RoutedEventArgs e)
        {
            _FormMain.ClientInfo.PaymentOperationType = PaymentOperationType.CreditPaymentBolcard;
            _FormMain.OpenForm(FormEnum.BolCard);
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.Products);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Title);
            LabelTestVersion.Visibility = App.TestVersion ? Visibility.Visible : Visibility.Collapsed;
            _FormMain = (MainWindow)Window.GetWindow(this);
        }
    }
}
