using System;
using CashInTerminal.BaseForms;
using CashInTerminal.Properties;

namespace CashInTerminal
{
    public partial class FormDebitClientInfo : FormMdiChild
    {
        private const string DATE_FORMAT = "dd MMMM yyyy";

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
            ChangeView(typeof(FormCurrencySelect));
        }

        private void FormDebitClientInfoLoad(object sender, EventArgs e)
        {
            lbl1.Text = Resources.Fullname + Resources.Colon + Utilities.FirstUpper(FormMain.ClientInfo.Client.FullName);
            lbl2.Text = Resources.PasportNumber + Resources.Colon + FormMain.ClientInfo.Client.PassportNumber;
            lbl3.Text = Resources.AccountNumber + Resources.Colon + FormMain.ClientInfo.Client.ClientAccount;
            lbl4.Text = Resources.CreditDate + Resources.Colon + FormMain.ClientInfo.Client.BeginDate.ToString(DATE_FORMAT);
            lbl5.Text = Resources.Currency + Resources.Colon + FormMain.ClientInfo.Client.Currency;
        }
    }
}
