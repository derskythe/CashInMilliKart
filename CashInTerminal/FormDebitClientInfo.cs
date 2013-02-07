using System;
using CashInTerminal.Enums;

namespace CashInTerminal
{
    public partial class FormDebitClientInfo : FormMdiChild
    {
        public FormDebitClientInfo()
        {
            InitializeComponent();
        }

        private void BtnDebitInfoBackClick(object sender, EventArgs e)
        {
            ChangeView(typeof(FormDebitSelectAccount));
        }

        private void BtnDebitInfoNextClick(object sender, EventArgs e)
        {
            ChangeView(typeof(FormMoneyInput));
        }

        private void FormDebitClientInfoLoad(object sender, EventArgs e)
        {
            //base.OnLoad(e);

            lblDebitInfoFullname.Text = @"TestFirstName TestLastName TestMiddleName";
            lblDebitInfoAccount.Text = FormMain.ClientInfo.AccountNumber;
            lblDebitInfoPassport.Text = FormMain.ClientInfo.Passport;
            lblDebitInfoDate.Text = DateTime.Now.AddMonths(-6).ToLongDateString();
            lblCurrency.Text = FormMain.ClientInfo.CurrentCurrency;
        }
    }
}
