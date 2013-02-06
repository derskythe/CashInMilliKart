using System;

namespace CashInTerminal
{
    public partial class FormPaySuccess : FormMdiChild
    {
        public FormPaySuccess()
        {
            InitializeComponent();
        }

        private void BtnSuccessNextClick(object sender, EventArgs e)
        {
            ChangeView(new FormProducts());
        }

        private void FormPaySuccessLoad(object sender, EventArgs e)
        {
            //base.OnLoad(e);

            lblSuccessTotalAmount.Text = FormMain.ClientInfo.CashCodeAmount + @" " + FormMain.ClientInfo.CurrentCurrency;
        }
    }
}
