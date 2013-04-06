using CashInTerminal.BaseForms;

namespace CashInTerminal
{
    public partial class FormOutOfOrder : FormMdiChild
    {
        public FormOutOfOrder()
        {
            InitializeComponent();
        }

        private void FormOutOfOrderLoad(object sender, System.EventArgs e)
        {
            HomeButton = false;
        }
    }
}
