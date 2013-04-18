using System.Windows;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageOutOfOrder.xaml
    /// </summary>
    public partial class PageOutOfOrder
    {
        private readonly MainWindow _FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageOutOfOrder()
        {
            InitializeComponent();
            _FormMain = (MainWindow)Window.GetWindow(this);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Name);
        }
    }
}
