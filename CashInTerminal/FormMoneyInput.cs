using System;
using System.Windows.Forms;
using CashInTerminal.Properties;

namespace CashInTerminal
{
    public partial class FormMoneyInput : FormMdiChild
    {
        private delegate void SetStackedAmountDelegate(object item);
        private delegate void EnableMoneyNextButtonDelegate(object item);

        private delegate void DisableBackButtonDelegate();

        public FormMoneyInput()
        {
            InitializeComponent();
        }

        private void BtnMoneyNextClick(object sender, EventArgs e)
        {
            btnMoneyNext.Enabled = false;
            try
            {
                StopCashcode();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            try
            {
                FormMain.Db.UpdateAmount(FormMain.ClientInfo.PaymentId, lblMoneyCurrency.Text, 1, Convert.ToInt32(lblMoneyTotal.Text));
                FormMain.Db.ConfirmTransaction(FormMain.ClientInfo.PaymentId);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            //lblSuccessTotalAmount.Text = lblMoneyTotal.Text + @" " + lblMoneyCurrency.Text;
            //PrintCheck();

            btnMoneyNext.Enabled = true;
            ChangeView(typeof(FormPaySuccess));
        }

        private void FormMoneyInputLoad(object sender, EventArgs e)
        {
            //base.OnLoad(e);

            lblMoneyCurrency.Text = FormMain.ClientInfo.CurrentCurrency;
            lblMoneyTotal.Text = @"0";
            FormMain.ClientInfo.CashCodeAmount = 0;

            FormMain.CcnetDevice.BillStacked += CcnetDeviceOnBillStacked;
            FormMain.CcnetDevice.ReadCommand += CcnetDeviceOnReadCommand;

            StartCashcode();
        }

        private void CcnetDeviceOnReadCommand(CCNETDeviceState ccnetDeviceState)
        {
            Log.Debug(ccnetDeviceState.ToString());
        }

        private void CcnetDeviceOnBillStacked(CCNETDeviceState ccnetDeviceState)
        {
            Log.Info(String.Format("Stacked {0} {1}", ccnetDeviceState.Nominal, ccnetDeviceState.Currency));

            try
            {
                DisableBackButton();
                EnableMoneyNextButton(null);

                SetStackedAmount(ccnetDeviceState.Amount);
                lock (FormMain.ClientInfo)
                {
                    FormMain.ClientInfo.CashCodeAmount = ccnetDeviceState.Amount;
                }

                FormMain.Db.InsertBanknote(FormMain.ClientInfo.PaymentId, ccnetDeviceState.Nominal,
                                           ccnetDeviceState.Currency, FormMain.ClientInfo.OrderNumber++);
                FormMain.Db.InsertTransactionBanknotes(ccnetDeviceState.Nominal, ccnetDeviceState.Currency,
                                                       FormMain.ClientInfo.TransactionId);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void FormMoneyInputFormClosing(object sender, FormClosingEventArgs e)
        {
            FormMain.CcnetDevice.BillStacked -= CcnetDeviceOnBillStacked;
            FormMain.CcnetDevice.ReadCommand -= CcnetDeviceOnReadCommand;
        }

        private void StartCashcode()
        {
            btnMoneyNext.Enabled = false;

            try
            {
                SetStackedAmount("0");

                FormMain.ClientInfo.OrderNumber = 0;
                FormMain.ClientInfo.PaymentId = FormMain.Db.InsertTransaction(FormMain.ClientInfo.ProductCode, FormMain.ClientInfo.CurrentCurrency, 1, 0,
                                                   Convert.ToInt32(Settings.Default.TerminalCode), false);
                FormMain.ClientInfo.TransactionId = String.Format("{0}{1}{2}", Settings.Default.TerminalCode,
                                               DateTime.Now.DayOfYear.ToString("000"), FormMain.ClientInfo.PaymentId);

                FormMain.Db.UpdateTransactionId(FormMain.ClientInfo.PaymentId, FormMain.ClientInfo.TransactionId);
                FormMain.Db.InsertPaymentValue(FormMain.ClientInfo.PaymentId, FormMain.ClientInfo.CreditAccountNumber, 0);
                FormMain.Db.InsertPaymentValue(FormMain.ClientInfo.PaymentId, FormMain.ClientInfo.Passport, 1);

                Log.Info("Starting transId: {0}, PaymentId: {1}", FormMain.ClientInfo.TransactionId, FormMain.ClientInfo.PaymentId);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
            FormMain.CcnetDevice.Poll();
            FormMain.CcnetDevice.Enable(FormMain.ClientInfo.CurrentCurrency.ToLower());
            FormMain.CcnetDevice.StartPool = true;
        }

        private void StopCashcode()
        {
            FormMain.CcnetDevice.StartPool = false;
            FormMain.CcnetDevice.Disable();
        }

        private void SetStackedAmount(object amount)
        {
            if (lblMoneyTotal.InvokeRequired)
            {
                lblMoneyTotal.Invoke(new SetStackedAmountDelegate(SetStackedAmount), amount);
            }
            else
            {
                lblMoneyTotal.Text = amount.ToString();
            }
        }

        private void EnableMoneyNextButton(object param)
        {
            if (btnMoneyNext.InvokeRequired)
            {
                btnMoneyNext.Invoke(new EnableMoneyNextButtonDelegate(EnableMoneyNextButton), param);
            }
            else
            {
                btnMoneyNext.Enabled = true;
            }
        }

        private void DisableBackButton()
        {
            if (btnBack.InvokeRequired)
            {
                btnBack.Invoke(new DisableBackButtonDelegate(DisableBackButton));
            }
            else
            {
                btnBack.Enabled = false;
            }
        }

        private void BtnBackClick(object sender, EventArgs e)
        {
            StopCashcode();

            switch (FormMain.ClientInfo.ProductCode)
            {
                case 1:
                    ChangeView(typeof(FormCreditClientInfo));
                    break;

                case 2:
                    ChangeView(typeof(FormDebitClientInfo));
                    break;
            }
        }
    }
}
