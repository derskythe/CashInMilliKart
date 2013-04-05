using System;
using System.Windows.Forms;
using CashInTerminal.BaseForms;
using CashInTerminal.CashIn;
using CashInTerminal.Properties;
using Containers.Enums;
using ResultCodes = CashInTerminal.CashIn.ResultCodes;

namespace CashInTerminal
{
    public partial class FormCreditByPassport2 : FormMdiChild
    {
        private TextBox _SelectedBox;

        private TextBox SelectedBox
        {
            get { return _SelectedBox ?? (_SelectedBox = txtClientCodeClient); }
            set { _SelectedBox = value; }
        }

        public FormCreditByPassport2()
        {
            InitializeComponent();
        }

        #region Input

        private void BtnClientCodeBackspaceClick(object sender, EventArgs e)
        {
            if (SelectedBox.Text.Length > 0)
            {
                SelectedBox.Text = SelectedBox.Text.Substring(0, SelectedBox.Text.Length - 1);
            }
        }

        private void BtnClientCode0Click(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnClientCodeClearClick(object sender, EventArgs e)
        {
            SelectedBox.Text = String.Empty;
        }

        private void BtnClientCode1Click(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnClientCode2Click(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnClientCode3Click(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnClientCode4Click(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnClientCode5Click(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnClientCode6Click(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnClientCode7Click(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnClientCode8Click(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnClientCode9Click(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnAClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnBClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnCClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnDClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnEClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnFClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnGClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnHClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnIClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnJClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnKClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnLClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnMClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnNClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnOClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnPClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnQClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnRClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnSClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnTClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnUClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnVClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnWClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnXClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnYClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnZClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnPlusClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void AddText(object sender)
        {
            if (sender == null)
            {
                Log.Error("Sender is null");
                return;
            }
            var t = (TextBox)sender;
            SelectedBox.Text += t.Text;
        }

        #endregion

        private void BtnClientCodeBackClick(object sender, EventArgs e)
        {
            ChangeView(typeof(FormCreditTypeSelect));
        }

        private void BtnClientCodeNextClick(object sender, EventArgs e)
        {
            if (SelectedBox.Text.Length > 4)
            {
                FormMain.ClientInfo.Passport = SelectedBox.Text;

                try
                {
                    DateTime now = DateTime.Now;
                    var request = new GetClientInfoRequest
                    {
                        PaymentOperationType = (int)FormMain.ClientInfo.PaymentOperationType,
                        CreditAccount = FormMain.ClientInfo.AccountNumber,
                        PasportNumber = FormMain.ClientInfo.Passport,
                        SystemTime = now,
                        TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                        Sign = Utilities.Sign(Settings.Default.TerminalCode, now, FormMain.ServerPublicKey)
                    };

                    var response = FormMain.Server.GetClientInfo(request);
                    if (response.ResultCodes != ResultCodes.Ok)
                    {
                        throw new Exception(response.Description);
                    }

                    if (response.Infos == null || response.Infos.Length == 0)
                    {
                        ChangeView(typeof(FormInvalidNumber));
                        return;
                    }

                    FormMain.Clients = response.Infos;

                    foreach (var clientInfo in response.Infos)
                    {
                        FormMain.ClientInfo.Client = clientInfo;
                        FormMain.ClientInfo.CurrentCurrency = clientInfo.Currency;
                        break;
                    }

                    if (FormMain.ClientInfo.PaymentOperationType == PaymentOperationType.CreditPaymentByClientCode)
                    {
                        ChangeView(typeof(FormCreditSelectAccount));
                    }
                    else
                    {
                        ChangeView(typeof(FormDebitSelectAccount));
                    }
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                    ChangeView(typeof(FormOutOfOrder));
                }
            }
        }

        private void TxtClientCodeClientClick(object sender, EventArgs e)
        {
            SelectedBox = txtClientCodeClient;
        }

        
    }
}
