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
            ChangePannel(pnlLanguage);
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

        private void btnRussian_Click(object sender, System.EventArgs e)
        {

        }

        private void btnAzeri_Click(object sender, System.EventArgs e)
        {

        }

        private void btnEnglish_Click(object sender, System.EventArgs e)
        {

        }
    }
}
