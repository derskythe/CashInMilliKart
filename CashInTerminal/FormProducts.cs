using System;

namespace CashInTerminal
{
    public partial class FormProducts : FormMdiChild
    {
        public FormProducts()
        {
            InitializeComponent();
        }

        private void BtnPayCreditClick(object sender, EventArgs e)
        {
            FormMain.ClientInfo.ProductCode = 1;
            ChangeView(new FormClientCode());
        }

        private void BtnPayDebitClick(object sender, EventArgs e)
        {
            FormMain.ClientInfo.ProductCode = 2;
            ChangeView(new FormDebitCardFull());
        }        
    }
}
