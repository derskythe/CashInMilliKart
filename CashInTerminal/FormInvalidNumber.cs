using System;
using CashInTerminal.BaseForms;
using CashInTerminal.CashIn;
using CashInTerminal.Enums;
using Containers.Enums;

namespace CashInTerminal
{
    public partial class FormInvalidNumber : FormMdiChild
    {
        public FormInvalidNumber()
        {
            InitializeComponent();
        }

        private void BtbBackClick(object sender, EventArgs e)
        {
            switch (FormMain.ClientInfo.PaymentOperationType)
            {
                case PaymentOperationType.CreditPaymentByClientCode:
                    ChangeView(typeof(BaseForms.FormEnterClientCode));
                    break;

                case PaymentOperationType.CreditPaymentByPassportAndAccount:
                    ChangeView(typeof(BaseForms.FormEnterByAlphabet));
                    break;

                case PaymentOperationType.CreditPaymentBolcard:
                    ChangeView(typeof(FormCreditByBolcard));
                    break;

                case PaymentOperationType.DebitPaymentByClientCode:
                    ChangeView(typeof(BaseForms.FormEnterClientCode));
                    break;

                case PaymentOperationType.DebitPaymentByPassportAndAccount:
                    ChangeView(typeof(BaseForms.FormEnterByAlphabet));
                    break;

                default:
                    ChangeView(typeof(FormProducts));
                    break;
            }
        }
    }
}
