using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CashInTerminal
{
    public partial class FormEncashment : FormMdiChild
    {
        public FormEncashment()
        {
            InitializeComponent();
        }

        private void BtnEncashmentPrintClick(object sender, EventArgs e)
        {
            try
            {
                var msg = new StringBuilder();
                foreach (var currency in FormMain.Currencies)
                {
                    var total = FormMain.Db.GetCasseteTotal(currency.Name);

                    msg.Append(total).Append(" ").Append(currency.Name).Append("\n");
                }

                Log.Info(msg.ToString());
                MessageBox.Show(msg.ToString());
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void BtnEncashmentFinishClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                FormMain.Db.DeleteCasseteBanknotes();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            Thread.Sleep(1000);
            FormMain.EncashmentMode = false;

            Cursor.Current = Cursors.Default;
            ChangeView(typeof(FormProducts));
        }
    }
}
