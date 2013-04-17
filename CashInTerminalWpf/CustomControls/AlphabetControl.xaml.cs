using System;
using System.Windows;
using System.Windows.Controls;
using NLog;

namespace CashInTerminalWpf.CustomControls
{
    /// <summary>
    /// Interaction logic for AlphabetControl.xaml
    /// </summary>
    public partial class AlphabetControl : UserControl
    {
        #region Events

        public static readonly RoutedEvent NewCharEvent = EventManager.RegisterRoutedEvent("NewAlphabetChar",
                                                                                           RoutingStrategy.Bubble,
                                                                                           typeof(NewCharEventHandler),
                                                                                           typeof(NumPadControl));
        public delegate void NewCharEventHandler
            (object sender, AlphabetPadRoutedEventArgs e);


        #endregion

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public AlphabetControl()
        {
            InitializeComponent();
        }

        private void BtnClick(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            Log.Info(btn.Content + " pressed");
            var newEventArgs = new AlphabetPadRoutedEventArgs(btn.Content.ToString()) { RoutedEvent = NewCharEvent };
            RaiseEvent(newEventArgs);
        }
    }

    #region AlphabetPadRoutedEventArgs

    public class AlphabetPadRoutedEventArgs : RoutedEventArgs
    {
        private readonly string _NewChar = String.Empty;

        public AlphabetPadRoutedEventArgs()
        {

        }

        public AlphabetPadRoutedEventArgs(string newChar)
        {
            _NewChar = newChar;
        }

        public string NewChar
        {
            get { return _NewChar; }
        }
    }

    #endregion
}
