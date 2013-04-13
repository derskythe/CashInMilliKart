using System;
using System.Windows.Forms;
using CashInTerminal.CashIn;
using CashInTerminal.Properties;

namespace CashInTerminal
{
    public partial class FormCreditByBolcardRetype : BaseForms.FormEnterClientCode
    {       
        public FormCreditByBolcardRetype()
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

                    //var response = FormMain.Server.GetClientInfo(request);

                    FormMain.InfoRequest = request;

                    ChangeView(typeof(FormProgress));

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

                    //foreach (var clientInfo in response.Infos)
                    //{
                    //    FormMain.ClientInfo.Client = clientInfo;
                    //    FormMain.ClientInfo.CurrentCurrency = clientInfo.Currency;
                    //    break;
                    //}

                    //ChangeView(typeof(FormCreditClientInfo));
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
            Label = Resources.BolLastFourRetype;
            MaxLength = 8;
        }
    }
}
