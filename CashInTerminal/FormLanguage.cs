using System;

namespace CashInTerminal
{
    public partial class FormLanguage : FormMdiChild
    {
        public FormLanguage()
        {
            InitializeComponent();
        }

        private void BtnAzeriClick(object sender, EventArgs e)
        {
            ChangeView(typeof(FormProducts));
        }

        private void BtnRussianClick(object sender, EventArgs e)
        {
            ChangeView(typeof(FormProducts));
        }

        private void BtnEnglishClick(object sender, EventArgs e)
        {
            ChangeView(typeof(FormProducts));
        }

        private void FormLanguage_Load(object sender, EventArgs e)
        {
            ChangeView(typeof(FormProducts));
        }
    }
}
