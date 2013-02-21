using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CashInTerminal
{
    public partial class FormEncashment : FormMdiChild
    {
        String _StreamToPrint = String.Empty;

        private const string PRINT_TEMPLATE =
            "    Инкассация терминала\n" + 
            "\n" + 
            "Дата: {0:t} {0:d}\n" +
            "-------------------------------------------------\n" + 
            "№ терминала: {1}\n" + 
            "{2}\n" +
            "-------------------------------------------------\n" + 
            "\n\n\n\n\n\n";

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

                _StreamToPrint = String.Format(PRINT_TEMPLATE, DateTime.Now, FormMain.TerminalInfo.Id, msg.ToString());

                Log.Info(msg.ToString());
                printDocument.Print();
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

        private void PrintDocumentPrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs ev)
        {
            float yPos = 0;
            int count = 0;
            //float leftMargin = ev.MarginBounds.Left;
            //float topMargin = ev.MarginBounds.Top;
            const float leftMargin = 25;
            const float topMargin = 0;
            var messageFont = new Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point);
            string line = null;

            // Calculate the number of lines per page.
            float linesPerPage = ev.MarginBounds.Height /
               messageFont.GetHeight(ev.Graphics);

            // Print each line of the file.
            yPos = topMargin;            
            ev.Graphics.DrawString(_StreamToPrint, messageFont, Brushes.Black,
                   leftMargin, yPos, new StringFormat());
        }
    }
}
