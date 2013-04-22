using System;
using CashInTerminal.Properties;

namespace CashInTerminal
{
    public partial class FormCreditByBolcard : BaseForms.FormEnterClientCode
    {
        public FormCreditByBolcard()
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
                FormMain.ClientInfo.AccountNumber = InputValue;
                ChangeView(typeof(FormCreditByBolcardRetype));
            }
        }

        private void FormCreditByBolcardLoad(object sender, EventArgs e)
        {
            Label = Resources.BolLastFour;
            MaxLength = 8;
        }
    }
}
