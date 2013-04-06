using System;
using CashInTerminal.BaseForms;
using Containers.Enums;

namespace CashInTerminal
{
    public partial class FormDebitPayType : FormMdiChild
    {
        public FormDebitPayType()
        {
            InitializeComponent();
        }

        private void BtnByCardFullClick(object sender, EventArgs e)
        {
            FormMain.ClientInfo.PaymentOperationType = PaymentOperationType.DebitPaymentByClientCode;
            ChangeView(typeof(FormCreditByClientCode));
        }

        private void BtnByCardAccountNumberClick(object sender, EventArgs e)
        {
            FormMain.ClientInfo.PaymentOperationType = PaymentOperationType.DebitPaymentByPassportAndAccount;
            ChangeView(typeof(FormCreditByPassport1));
        }

        //private void BtnCurrentClick(object sender, EventArgs e)
        //{
        //    FormMain.ClientInfo.DebitPayType = DebitPayType.Current;
        //    ChangeView(typeof(FormClientCode));
        //}

        //private void BtnDepositClick(object sender, EventArgs e)
        //{
        //    FormMain.ClientInfo.DebitPayType = DebitPayType.DebitAccount;
        //    ChangeView(typeof(FormClientCode));
        //}

        private void BtnBackClick(object sender, EventArgs e)
        {
            ChangeView(typeof(FormProducts));
        }

        private void FormDebitPayTypeLoad(object sender, EventArgs e)
        {
            HomeButton = false;
        }
    }
}
