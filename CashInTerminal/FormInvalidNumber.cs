using System;
using CashInTerminal.BaseForms;
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
                    ChangeView(typeof(FormCreditByClientCode));
                    break;

                case PaymentOperationType.CreditPaymentByPassportAndAccount:
                    ChangeView(typeof(FormCreditByPassport1));
                    break;

                case PaymentOperationType.CreditPaymentBolcard:
                    ChangeView(typeof(FormCreditByBolcard));
                    break;

                case PaymentOperationType.DebitPaymentByClientCode:
                    ChangeView(typeof(FormCreditByClientCode));
                    break;

                case PaymentOperationType.DebitPaymentByPassportAndAccount:
                    ChangeView(typeof(FormCreditByPassport1));
                    break;

                default:
                    ChangeView(typeof(FormProducts));
                    break;
            }
        }
    }
}
