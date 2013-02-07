using System;

namespace CashInTerminal
{
    public partial class FormCreditClientInfo : FormMdiChild
    {
        public FormCreditClientInfo()
        {
            InitializeComponent();
        }

        private void BtnCreditInfoBackClick(object sender, EventArgs e)
        {
            ChangeView(typeof(FormCreditSelectAccount));
        }

        private void BtnCreditInfoNextClick(object sender, EventArgs e)
        {
            ChangeView(typeof(FormMoneyInput));
        }

        private void FormCreditClientInfoLoad(object sender, EventArgs e)
        {
            //base.OnLoad(e);

            lblCreditInfoFullname.Text = "TestFirstName TestLastName TestMiddleName";
            lblCreditInfoAccountNumber.Text = FormMain.ClientInfo.AccountNumber;
            lblCreditInfoPassport.Text = FormMain.ClientInfo.Passport;
            lblCreditInfoProductName.Text = "Test product";
            lblCreditInfoCurrentAmount.Text = "1000 AZN";
            lblCreditInfoTotalAmount.Text = "1000 AZN";
            lblCreditInfoDate.Text = DateTime.Now.AddMonths(6).ToLongDateString();
        }
    }
}
