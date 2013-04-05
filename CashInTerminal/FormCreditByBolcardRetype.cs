using System;
using System.Windows.Forms;
using CashInTerminal.BaseForms;
using CashInTerminal.CashIn;
using CashInTerminal.Properties;

namespace CashInTerminal
{
    public partial class FormCreditByBolcardRetype : FormMdiChild
    {
        private TextBox _SelectedBox;

        private TextBox SelectedBox
        {
            get { return _SelectedBox ?? (_SelectedBox = txtClientCodeClient); }
            set { _SelectedBox = value; }
        }

        public FormCreditByBolcardRetype()
        {
            InitializeComponent();
        }

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

        private void BtnClientCodeBackClick(object sender, EventArgs e)
        {
            ChangeView(typeof(FormCreditTypeSelect));
        }

        private void BtnClientCodeNextClick(object sender, EventArgs e)
        {
            if (SelectedBox.Text.Length > 4)
            {
                if (SelectedBox.Text != FormMain.ClientInfo.AccountNumber)
                {
                    ChangeView(typeof (FormInvalidNumber));
                    return;
                }
                try
                {
                    DateTime now = DateTime.Now;
                    var request = new GetClientInfoRequest
                        {
                            PaymentOperationType = (int)FormMain.ClientInfo.PaymentOperationType,
                            Bolcard8Digits = FormMain.ClientInfo.AccountNumber,
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

                    ChangeView(typeof(FormCreditClientInfo));
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

        private void FormCreditByClientCodeRetypeLoad(object sender, EventArgs e)
        {

        }
    }
}
