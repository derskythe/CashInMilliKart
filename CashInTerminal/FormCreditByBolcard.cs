using System;
using System.Windows.Forms;
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

        private void FormCreditByBolcard_Load(object sender, EventArgs e)
        {
            Label = Resources.BolLastFour;
        }
    }
}
