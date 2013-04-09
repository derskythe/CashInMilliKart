using System;
using System.Threading;
using CashInTerminal.CashIn;
using Containers.Enums;
using ResultCodes = CashInTerminal.CashIn.ResultCodes;

namespace CashInTerminal
{
    public partial class FormProgress : BaseForms.FormMdiChild
    {
        private Thread _WorkerThread;

        public FormProgress()
        {
            InitializeComponent();
        }

        private void FormProgressLoad(object sender, EventArgs e)
        {
            HomeButton = false;
            _WorkerThread = new Thread(DoWork);
            _WorkerThread.Start(FormMain.InfoRequest);
        }

        private void DoWork(object value)
        {
            try
            {
                var request = (GetClientInfoRequest) value;

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

                if (FormMain.ClientInfo.PaymentOperationType == PaymentOperationType.CreditPaymentByClientCode || FormMain.ClientInfo.PaymentOperationType == PaymentOperationType.CreditPaymentByPassportAndAccount ||FormMain.ClientInfo.PaymentOperationType == PaymentOperationType.CreditPaymentBolcard)
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
}
