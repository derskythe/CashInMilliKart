﻿using System;
using System.Windows;
using CashInTerminalWpf.CashIn;
using CashInTerminalWpf.Enums;
using CashInTerminalWpf.Properties;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageClientCodeRetype.xaml
    /// </summary>
    public partial class PageClientCodeRetype
    {
        private MainWindow _FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageClientCodeRetype()
        {
            InitializeComponent();
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.CreditPaymentTypeSelect);
        }

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            if (ClientNumber.Text.Length > 4)
            {
                Log.Info("Input value: " + ClientNumber.Text);
                if (ClientNumber.Text != _FormMain.ClientInfo.AccountNumber)
                {
                    _FormMain.OpenForm(FormEnum.InvalidNumber);
                    return;
                }

                try
                {
                    DateTime now = DateTime.Now;
                    var request = new GetClientInfoRequest
                    {
                        PaymentOperationType = (int)_FormMain.ClientInfo.PaymentOperationType,
                        ClientCode = _FormMain.ClientInfo.AccountNumber,
                        SystemTime = now,
                        TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                        Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _FormMain.ServerPublicKey),
                        Ticks = now.Ticks
                    };

                    _FormMain.LongRequestType = LongRequestType.CreditDebitInfo;
                    _FormMain.InfoRequest = request;
                    _FormMain.OpenForm(FormEnum.Progress);
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                    _FormMain.OpenForm(FormEnum.OutOfOrder);
                }
            }
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Title);
            _FormMain = (MainWindow)Window.GetWindow(this);

            ControlNumPad.AddHandler(NumPadControl.NewCharEvent, new NumPadControl.NewCharEventHandler(ControlNumPadOnNewChar));
            ControlNumPad.AddHandler(NumPadControl.BackspaceEvent, new NumPadControl.BackspaceEventHandler(ControlNumPadOnBackSpace));
            ControlNumPad.AddHandler(NumPadControl.ClearAllEvent, new NumPadControl.ClearAllEventHandler(ControlNumPadOnClearAll));
        }

        private void ControlNumPadOnClearAll(object sender, NumPadRoutedEventArgs numPadRoutedEventArgs)
        {
            ClientNumber.Text = String.Empty;
        }

        private void ControlNumPadOnBackSpace(object sender, NumPadRoutedEventArgs args)
        {
            try
            {
                if (ClientNumber.Text.Length > 0)
                {
                    ClientNumber.Text = ClientNumber.Text.Substring(0, ClientNumber.Text.Length - 1);
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void ControlNumPadOnNewChar(object sender, NumPadRoutedEventArgs args)
        {
            ClientNumber.Text += args.NewChar;
        }

        private void ButtonHomeClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.Products);
        }
    }
}
