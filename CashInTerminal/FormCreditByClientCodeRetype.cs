using System;
using CashInTerminal.CashIn;
using CashInTerminal.Properties;
using Containers.Enums;
using ResultCodes = CashInTerminal.CashIn.ResultCodes;

namespace CashInTerminal
{
    public partial class FormCreditByClientCodeRetype : BaseForms.FormEnterClientCode
    {
        public FormCreditByClientCodeRetype()
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
                if (InputValue != FormMain.ClientInfo.AccountNumber)
                {
                    ChangeView(typeof(FormInvalidNumber));
                    return;
                }

                try
                {
                    DateTime now = DateTime.Now;
                    var request = new GetClientInfoRequest
                        {
                            PaymentOperationType = (int)FormMain.ClientInfo.PaymentOperationType,
                            ClientCode = FormMain.ClientInfo.AccountNumber,
                            SystemTime = now,
                            TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                            Sign = Utilities.Sign(Settings.Default.TerminalCode, now, FormMain.ServerPublicKey)
                        };

                    FormMain.InfoRequest = request;

                    ChangeView(typeof(FormProgress));

                    //var response = FormMain.Server.GetClientInfo(request);
                    //if (response.ResultCodes != ResultCodes.Ok)
                    //{
                    //    throw new Exception(response.Description);
                    //}

                    //if (response.Infos == null || response.Infos.Length == 0)
                    //{
                    //    ChangeView(typeof(FormInvalidNumber));
                    //    return;
                    //}

                    //FormMain.Clients = response.Infos;
                    //if (FormMain.ClientInfo.PaymentOperationType == PaymentOperationType.CreditPaymentByClientCode)
                    //{
                    //    ChangeView(typeof(FormCreditSelectAccount));
                    //}
                    //else
                    //{
                    //    ChangeView(typeof(FormDebitSelectAccount));
                    //}
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                    ChangeView(typeof(FormOutOfOrder));
                }
            }
        }

        private void FormCreditByClientCodeRetypeLoad(object sender, EventArgs e)
        {
            Label = Resources.AccountNumberRetype;
        }
    }
}
