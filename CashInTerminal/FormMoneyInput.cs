using System;
using System.Threading;
using System.Windows.Forms;
using CashInTerminal.BaseForms;
using CashInTerminal.Enums;
using CashInTerminal.Properties;
using Containers.CashCode;

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
            Thread.Sleep(250);
            
            if (FormMain.CcnetDevice.DeviceState.StateCode == CCNETCommand.Stacking)
            {
                return;
            }

            btnMoneyNext.Enabled = false;

            Thread.Sleep(250);
            try
            {
                Log.Debug("Stopping cashcode");
                StopCashcode();
                Log.Debug("Stoped cashcode");
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            try
            {
                Log.Debug("Update DB");

                lock (FormMain.ClientInfo)
                {
                    FormMain.Db.UpdateAmount(
                        FormMain.ClientInfo.PaymentId,
                        lblMoneyCurrency.Text,
                        1,
                        Convert.ToInt32(FormMain.ClientInfo.CashCodeAmount));

                    FormMain.Db.ConfirmTransaction(FormMain.ClientInfo.PaymentId);
                }
                
                Log.Debug("Done");
            }
            catch (Exception exp)
            {
                Log.FatalException(exp.Message, exp);
            }

            //lblSuccessTotalAmount.Text = lblMoneyTotal.Text + @" " + lblMoneyCurrency.Text;
            //PrintCheck();
            Log.Debug("Change view");
            btnMoneyNext.Enabled = true;
            ChangeView(typeof(FormPaySuccess));
        }

        private void FormMoneyInputLoad(object sender, EventArgs e)
        {
            //base.OnLoad(e);
            HomeButton = false;

            if (FormMain.ClientInfo.CurrentCurrency != FormMain.ClientInfo.Client.Currency)
            {
                lblComission.Visible = true;
            }

            lblMoneyCurrency.Text = FormMain.ClientInfo.CurrentCurrency;
            lblMoneyTotal.Text = @"0";
            FormMain.ClientInfo.CashCodeAmount = 0;

            FormMain.CcnetDevice.BillStacked += CcnetDeviceOnBillStacked;
            FormMain.CcnetDevice.ReadCommand += CcnetDeviceOnReadCommand;

            var startCashCodeThread = new Thread(StartCashcode);
            startCashCodeThread.Start();
        }

        private void CcnetDeviceOnReadCommand(CCNETDeviceState ccnetDeviceState)
        {
            //Log.Debug(ccnetDeviceState.ToString());
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

                FormMain.Db.InsertBanknote(
                                            FormMain.ClientInfo.PaymentId,
                                            ccnetDeviceState.Nominal,
                                            ccnetDeviceState.Currency,
                                            FormMain.ClientInfo.OrderNumber++);
                FormMain.Db.InsertTransactionBanknotes(
                    ccnetDeviceState.Nominal,
                    ccnetDeviceState.Currency,
                                                       FormMain.ClientInfo.TransactionId);
            }
            catch (Exception exp)
            {
                Log.FatalException(exp.Message, exp);
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
                FormMain.ClientInfo.PaymentId = FormMain.Db.InsertTransaction(Convert.ToInt64(FormMain.ClientInfo.Product.Id),
                                                                              FormMain.ClientInfo.CurrentCurrency,
                                                                              1,
                                                                              0,
                                                                              Convert.ToInt32(
                                                                                  Settings.Default.TerminalCode),
                                                                              FormMain.ClientInfo.Client.CreditNumber,
                                                                              (int)FormMain.ClientInfo.PaymentOperationType,
                                                                              false);
                int termCode = Convert.ToInt32(Settings.Default.TerminalCode);
                FormMain.ClientInfo.TransactionId = String.Format("{0}{1}{2}", termCode.ToString("000"),
                                               DateTime.Now.DayOfYear.ToString("000"), FormMain.ClientInfo.PaymentId);

                FormMain.Db.UpdateTransactionId(FormMain.ClientInfo.PaymentId, FormMain.ClientInfo.TransactionId);
                FormMain.Db.InsertPaymentValue(FormMain.ClientInfo.PaymentId, FormMain.ClientInfo.Client.ClientAccount, 0);
                FormMain.Db.InsertPaymentValue(FormMain.ClientInfo.PaymentId, FormMain.ClientInfo.Client.PassportNumber, 1);

                Log.Info("Starting transId: {0}, PaymentId: {1}", FormMain.ClientInfo.TransactionId, FormMain.ClientInfo.PaymentId);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            try
            {
                FormMain.CcnetDevice.Poll();
                FormMain.CcnetDevice.Enable(FormMain.ClientInfo.CurrentCurrency.ToLower());
                FormMain.CcnetDevice.StartPool = true;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
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
            try
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
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void DisableBackButton()
        {
            try
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
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void BtnBackClick(object sender, EventArgs e)
        {
            StopCashcode();

            switch ((CheckTemplateTypes)FormMain.ClientInfo.Product.CheckType)
            {
                case CheckTemplateTypes.CreditPayment:
                    ChangeView(typeof(FormCreditClientInfo));
                    break;

                case CheckTemplateTypes.DebitPayment:
                    ChangeView(typeof(FormDebitClientInfo));
                    break;
            }
        }
    }
}
