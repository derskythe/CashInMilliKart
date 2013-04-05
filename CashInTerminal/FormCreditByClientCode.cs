using System;
using CashInTerminal.Properties;
using Containers.Enums;

namespace CashInTerminal
{
    public partial class FormCreditByClientCode : BaseForms.FormEnterClientCode
    {

        public FormCreditByClientCode()
        {
            InitializeComponent();
        }

        protected override void BtnBack()
        {
            ChangeView(FormMain.ClientInfo.PaymentOperationType == PaymentOperationType.DebitPaymentByClientCode
                           ? typeof(FormDebitPayType)
                           : typeof(FormCreditTypeSelect));
        }

        protected override void BtnNext()
        {
            if (InputValue.Length > 4)
            {
                FormMain.ClientInfo.AccountNumber = InputValue;
                ChangeView(typeof(FormCreditByClientCodeRetype));
            }
        }

        private void FormCreditByClientCodeLoad(object sender, EventArgs e)
        {
            Label = Resources.AccountNumber;
        }
    }
}
