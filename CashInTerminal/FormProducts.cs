using System;
using System.Windows.Forms;

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
            ChangeView(typeof(FormCreditTypeSelect));
        }

        private void BtnPayDebitClick(object sender, EventArgs e)
        {
            FormMain.ClientInfo.ProductCode = 2;
            ChangeView(typeof(FormDebitPayType));
        }

        private void FormProductsLoad(object sender, EventArgs e)
        {
            Log.Debug(String.Format("ClientRect: {0}, ClientSize: {1}", ClientRectangle, ClientSize));
        }
    }
}
