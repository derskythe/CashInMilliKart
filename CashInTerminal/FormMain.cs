using System.Windows.Forms;
using NLog;

namespace CashInTerminal
{
    public partial class FormMain : Form
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public FormMain()
        {
            InitializeComponent();

            Log.Info("Started");
        }

        private void FormMainLoad(object sender, System.EventArgs e)
        {
            ChangePannel(pnlOutOfOrder);
        }

        private void ChangePannel(Panel currentPannel)
        {
            foreach (Control childControl in Controls)
            {   
                if (childControl is Panel)
                {
                    if (childControl == currentPannel)
                    {
                        childControl.Visible = true;
                        childControl.Dock = DockStyle.Fill;
                    }
                    else
                    {
                        childControl.Visible = false;
                    }
                }
            }
        }

        private void BtnRussianClick(object sender, System.EventArgs e)
        {

        }

        private void BtnAzeriClick(object sender, System.EventArgs e)
        {

        }

        private void BtnEnglishClick(object sender, System.EventArgs e)
        {

        }

        private void BtnPayCreditClick(object sender, System.EventArgs e)
        {

        }

        private void BtnPayDebitClick(object sender, System.EventArgs e)
        {

        }

        private void BtnClientCodeBackClick(object sender, System.EventArgs e)
        {

        }

        private void BtnClientCodeNextClick(object sender, System.EventArgs e)
        {

        }

        private void BtnClientCodeBackspaceClick(object sender, System.EventArgs e)
        {

        }

        private void BtnClientCode0Click(object sender, System.EventArgs e)
        {

        }

        private void BtnClientCodeClearClick(object sender, System.EventArgs e)
        {

        }

        private void BtnClientCode1Click(object sender, System.EventArgs e)
        {

        }

        private void BtnClientCode2Click(object sender, System.EventArgs e)
        {

        }

        private void BtnClientCode3Click(object sender, System.EventArgs e)
        {

        }

        private void btnClientCode4_Click(object sender, System.EventArgs e)
        {

        }

        private void BtnClientCode5Click(object sender, System.EventArgs e)
        {

        }

        private void BtnClientCode6Click(object sender, System.EventArgs e)
        {

        }

        private void BtnClientCode7Click(object sender, System.EventArgs e)
        {

        }

        private void BtnClientCode8Click(object sender, System.EventArgs e)
        {

        }

        private void BtnClientCode9Click(object sender, System.EventArgs e)
        {

        }

        private void BtnCreditInfoBackClick(object sender, System.EventArgs e)
        {

        }

        private void BtnCreditInfoNextClick(object sender, System.EventArgs e)
        {

        }

        private void BtnDebitInfoBackClick(object sender, System.EventArgs e)
        {

        }

        private void BtnDebitInfoNextClick(object sender, System.EventArgs e)
        {

        }

        private void BtnEncashmentPrintClick(object sender, System.EventArgs e)
        {

        }

        private void BtnEncashmentFinishClick(object sender, System.EventArgs e)
        {

        }
    }
}
