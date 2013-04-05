using System;
using CashInTerminal.Properties;
using Containers.Enums;

namespace CashInTerminal
{
    public partial class FormCreditByPassport1 : BaseForms.FormEnterByAlphabet
    {
        public FormCreditByPassport1()
        {
            InitializeComponent();
        }

        private void FormCreditByPassport1Load(object sender, EventArgs e)
        {
            // Номер счета
            Label = Resources.AccountNumber;
        }

        protected override void BtnBack()
        {
            Log.Debug("Back inherited");
            ChangeView(FormMain.ClientInfo.PaymentOperationType == PaymentOperationType.DebitPaymentByPassportAndAccount
                           ? typeof(FormDebitPayType)
                           : typeof(FormCreditTypeSelect));
        }

        protected override void BtnNext()
        {
            if (!String.IsNullOrEmpty(InputValue) && InputValue.Length > 4)
            {
                FormMain.ClientInfo.AccountNumber = InputValue;
                ChangeView(typeof(FormCreditByPassport2));
            }
        }

        //protected void BtnClientCodeBackClick(object sender, EventArgs e)
        //{
        //    //ChangeView(typeof(FormCreditTypeSelect));

        //    ChangeView(FormMain.ClientInfo.PaymentOperationType == PaymentOperationType.DebitPaymentByPassportAndAccount
        //                   ? typeof(FormDebitPayType)
        //                   : typeof(FormCreditTypeSelect));
        //}

        //protected void BtnClientCodeNextClick(object sender, EventArgs e)
        //{
        //    if (SelectedBox.Text.Length > 4)
        //    {
        //        FormMain.ClientInfo.AccountNumber = SelectedBox.Text;
        //        ChangeView(typeof(FormCreditByPassport2));
        //    }
        //}      
    }
}
