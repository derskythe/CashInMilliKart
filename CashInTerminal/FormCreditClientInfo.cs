using System;
using CashInTerminal.CashIn;
using CashInTerminal.Properties;

namespace CashInTerminal
{
    public partial class FormCreditClientInfo : FormMdiChild
    {
        const string DATE_FORMAT = "dd MMMM yyyy";
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
            switch (FormMain.ClientInfo.GetClientInfoType)
            {
                case GetClientInfoType.ByClientCode:
                    lbl1.Text = Resources.Fullname + Resources.Colon + Utilities.FirstUpper(FormMain.ClientInfo.Client.FullName);
                    lbl2.Text = Resources.PasportNumber + Resources.Colon + FormMain.ClientInfo.Client.PassportNumber;
                    lbl3.Text = Resources.AccountNumber + Resources.Colon + FormMain.ClientInfo.Client.ClientAccount;
                    lbl4.Text = Resources.CreditDate + Resources.Colon + FormMain.ClientInfo.Client.BeginDate.ToString(DATE_FORMAT);
                    lbl5.Text = Resources.CreditAmount + Resources.Colon + FormMain.ClientInfo.Client.CreditAmount + @" " + FormMain.ClientInfo.Client.Currency;
                    lbl6.Text = Resources.Currency + Resources.Colon + FormMain.ClientInfo.Client.Currency;
                    lbl7.Text = Resources.CreditAmountToPay + Resources.Colon + FormMain.ClientInfo.Client.AmountLate + @" " + FormMain.ClientInfo.Client.Currency;
                    break;

                case GetClientInfoType.ByPasportAndCreditNumber:
                    lbl1.Text = Resources.Fullname + Resources.Colon + Utilities.FirstUpper(FormMain.ClientInfo.Client.FullName);
                    lbl2.Text = Resources.PasportNumber + Resources.Colon + FormMain.ClientInfo.Client.PassportNumber;
                    lbl3.Text = Resources.AccountNumber + Resources.Colon + FormMain.ClientInfo.Client.ClientAccount;
                    lbl4.Text = Resources.CreditDate + Resources.Colon + FormMain.ClientInfo.Client.BeginDate.ToString(DATE_FORMAT);
                    lbl5.Text = Resources.CreditAmount + Resources.Colon + FormMain.ClientInfo.Client.CreditAmount + @" " + FormMain.ClientInfo.Client.Currency;
                    lbl6.Text = Resources.Currency + Resources.Colon + FormMain.ClientInfo.Client.Currency;
                    lbl7.Text = Resources.CreditAmountToPay + Resources.Colon + FormMain.ClientInfo.Client.AmountLate + @" " + FormMain.ClientInfo.Client.Currency;
                    break;

                case GetClientInfoType.Bolcard:
                    lbl1.Text = Resources.Fullname + Resources.Colon + Utilities.FirstUpper(FormMain.ClientInfo.Client.FullName);
                    lbl2.Text = Resources.PasportNumber + Resources.Colon + FormMain.ClientInfo.Client.PassportNumber;
                    lbl3.Text = Resources.AccountNumber + Resources.Colon + FormMain.ClientInfo.Client.ClientAccount;
                    lbl4.Text = Resources.CreditDate + Resources.Colon + FormMain.ClientInfo.Client.BeginDate.ToString(DATE_FORMAT);
                    lbl5.Text = Resources.CreditAmount + Resources.Colon + FormMain.ClientInfo.Client.CreditAmount + @" " + FormMain.ClientInfo.Client.Currency;
                    lbl6.Text = Resources.Currency + Resources.Colon + FormMain.ClientInfo.Client.Currency;
                    lbl7.Text = Resources.CreditAmountToPay + Resources.Colon + FormMain.ClientInfo.Client.AmountLate + @" " + FormMain.ClientInfo.Client.Currency;
                    break;
            }
        }
    }
}
