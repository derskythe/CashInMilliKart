using System;
using System.Windows;
using System.Windows.Controls;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for NumPadControl.xaml
    /// </summary>
    public partial class NumPadControl
    {
        #region Events

        public static readonly RoutedEvent NewCharEvent = EventManager.RegisterRoutedEvent("NewChar",
                                                                                           RoutingStrategy.Bubble,
                                                                                           typeof(NewCharEventHandler),
                                                                                           typeof(NumPadControl));

        public static readonly RoutedEvent BackspaceEvent = EventManager.RegisterRoutedEvent("Backspace",
                                                                                             RoutingStrategy.Bubble,
                                                                                             typeof(BackspaceEventHandler),
                                                                                             typeof(NumPadControl));

        public static readonly RoutedEvent ClearAllEvent = EventManager.RegisterRoutedEvent("ClearAll",
                                                                                            RoutingStrategy.Bubble,
                                                                                            typeof(ClearAllEventHandler),
                                                                                            typeof(NumPadControl));

        public delegate void BackspaceEventHandler
           (object sender, NumPadRoutedEventArgs e);

        public delegate void NewCharEventHandler
            (object sender, NumPadRoutedEventArgs e);

        public delegate void ClearAllEventHandler
            (object sender, NumPadRoutedEventArgs e);

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        //public event BackspaceEventHandler BackSpace
        //{
        //    add { AddHandler(BackspaceEvent, value); }
        //    remove { RemoveHandler(BackspaceEvent, value); }
        //}

        //public event ClearAllEventHandler ClearAll
        //{
        //    add { AddHandler(ClearAllEvent, value); }
        //    remove { RemoveHandler(ClearAllEvent, value); }
        //}

        //public event NewCharEventHandler NewChar
        //{
        //    add { AddHandler(NewCharEvent, value); }
        //    remove { RemoveHandler(NewCharEvent, value); }
        //}

        #endregion

        public NumPadControl()
        {
            InitializeComponent();
        }

        private void RaiseNewCharEvent(Button sender)
        {
            Log.Info(sender.Content + " pressed");

            var newEventArgs = new NumPadRoutedEventArgs(sender.Content.ToString()) { RoutedEvent = NewCharEvent };
            RaiseEvent(newEventArgs);
        }

        private void RaiseBackSpaceEvent()
        {
            Log.Info("BackSpace pressed");

            var newEventArgs = new NumPadRoutedEventArgs { RoutedEvent = BackspaceEvent };
            RaiseEvent(newEventArgs);
        }

        private void RaiseClearAllEvent()
        {
            Log.Info("Clear All pressed");

            var newEventArgs = new NumPadRoutedEventArgs { RoutedEvent = ClearAllEvent };
            RaiseEvent(newEventArgs);
        }

        private void Btn7Click(object sender, RoutedEventArgs e)
        {
            RaiseNewCharEvent((Button)sender);
        }

        private void Btn8Click(object sender, RoutedEventArgs e)
        {
            RaiseNewCharEvent((Button)sender);
        }

        private void Btn9Click(object sender, RoutedEventArgs e)
        {
            RaiseNewCharEvent((Button)sender);
        }

        private void Btn6Click(object sender, RoutedEventArgs e)
        {
            RaiseNewCharEvent((Button)sender);
        }

        private void Btn5Click(object sender, RoutedEventArgs e)
        {
            RaiseNewCharEvent((Button)sender);
        }

        private void Btn4Click(object sender, RoutedEventArgs e)
        {
            RaiseNewCharEvent((Button)sender);
        }

        private void Btn3Click(object sender, RoutedEventArgs e)
        {
            RaiseNewCharEvent((Button)sender);
        }

        private void Btn2Click(object sender, RoutedEventArgs e)
        {
            RaiseNewCharEvent((Button)sender);
        }

        private void Btn1Click(object sender, RoutedEventArgs e)
        {
            RaiseNewCharEvent((Button)sender);
        }

        private void BtnBackSpaceClick(object sender, RoutedEventArgs e)
        {
            RaiseBackSpaceEvent();
        }

        private void Btn0Click(object sender, RoutedEventArgs e)
        {
            RaiseNewCharEvent((Button)sender);
        }

        private void BtnClearClick(object sender, RoutedEventArgs e)
        {
            RaiseClearAllEvent();
        }
    }

    #region NumPadRoutedEventArgs

    public class NumPadRoutedEventArgs : RoutedEventArgs
    {
        private readonly string _NewChar = String.Empty;

        public NumPadRoutedEventArgs()
        {

        }

        public NumPadRoutedEventArgs(string newChar)
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
