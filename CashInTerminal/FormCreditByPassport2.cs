using System;
using CashInTerminal.CashIn;
using CashInTerminal.Properties;
using Containers.Enums;
using ResultCodes = CashInTerminal.CashIn.ResultCodes;

namespace CashInTerminal
{
    public partial class FormCreditByPassport2 : BaseForms.FormEnterByAlphabet
    {
        public FormCreditByPassport2()
        {
            InitializeComponent();
        }

        protected override void BtnBack()
        {
            ChangeView(typeof(FormCreditTypeSelect));
        }

        protected override void BtnNext()
        {
            if (InputValue.Length > 4)
            {
                FormMain.ClientInfo.Passport = InputValue;

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

        private void FormCreditByPassport2Load(object sender, EventArgs e)
        {
            Label = Resources.PasportInfo;
        }        
    }
}
