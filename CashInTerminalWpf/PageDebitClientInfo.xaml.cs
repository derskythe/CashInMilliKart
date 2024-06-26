﻿using System;
using System.Windows;
using CashInTerminalWpf.Enums;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageDebitClientInfo.xaml
    /// </summary>
    public partial class PageDebitClientInfo
    {
        private MainWindow _FormMain;
        private const string DATE_FORMAT = "dd MMMM yyyy";

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageDebitClientInfo()
        {
            InitializeComponent();            
        }

        private void ButtonHomeClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.Products);
        }

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.CurrencySelect);
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.DebitSelectAccount);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Title);
            _FormMain = (MainWindow)Window.GetWindow(this);

            try
            {
                Label1.Content = Properties.Resources.Fullname + Properties.Resources.Colon + Utilities.FirstUpper(_FormMain.ClientInfo.Client.FullName);
                Label2.Content = Properties.Resources.PasportNumber + Properties.Resources.Colon + _FormMain.ClientInfo.Client.PassportNumber;
                Label3.Content = Properties.Resources.AccountNumber + Properties.Resources.Colon + _FormMain.ClientInfo.Client.ClientAccount;
                Label4.Content = Properties.Resources.CreditDate + Properties.Resources.Colon + _FormMain.ClientInfo.Client.BeginDate.ToString(DATE_FORMAT);
                Label5.Content = Properties.Resources.Currency + Properties.Resources.Colon + _FormMain.ClientInfo.Client.Currency;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }
    }
}
