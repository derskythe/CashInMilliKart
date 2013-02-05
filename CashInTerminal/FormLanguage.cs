using System;

namespace CashInTerminal
{
    public partial class FormLanguage : FormMdiChild
    {
        public FormLanguage()
        {
            InitializeComponent();
        }

        private void btnAzeri_Click(object sender, EventArgs e)
        {
            ChangeView(new FormProducts());
        }

        private void btnRussian_Click(object sender, EventArgs e)
        {
            ChangeView(new FormProducts());
        }

        private void btnEnglish_Click(object sender, EventArgs e)
        {
            ChangeView(new FormProducts());
        }
    }
}
