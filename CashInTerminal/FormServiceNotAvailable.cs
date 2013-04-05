using System;
using CashInTerminal.BaseForms;

namespace CashInTerminal
{
    public partial class FormServiceNotAvailable : FormMdiChild
    {
        public FormServiceNotAvailable()
        {
            InitializeComponent();
        }

        private void BtnBackClick(object sender, EventArgs e)
        {
            FormMain.OpenForm(typeof(FormProducts));
        }
    }
}
