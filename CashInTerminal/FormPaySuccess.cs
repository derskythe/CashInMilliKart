using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Threading;

namespace CashInTerminal
{
    public partial class FormPaySuccess : FormMdiChild
    {
        String _StreamToPrint = String.Empty;
        private const string PRINT_TEMPLATE =
            "    Оплата посредством терминала\n" +
            "\n" +
            "Дата: {0:t} {0:d}\n" +
            "-------------------------------------------------\n" +
            "№ терминала: {1}\n" +
            "Код: {2}\n" +
            "Код транзакции: {3}\n" +
            "Операция: {4}\n" +
            "Номер счета: {5}\n" +
            "Сумма: {6}\n" +
            "-------------------------------------------------\n" +
            "\n" +
            "\n" +
            "Спасибо\n\n\n\n";

        public delegate string AsyncMethodCaller();

        public FormPaySuccess()
        {
            InitializeComponent();
        }

        private void BtnSuccessNextClick(object sender, EventArgs e)
        {
            ChangeView(typeof(FormProducts));
        }

        private void FormPaySuccessLoad(object sender, EventArgs e)
        {
            Log.Debug("FormPaySuccessLoad");
            //base.OnLoad(e);
            if (FormMain.ClientInfo == null)
            {
                Log.Error("ClientInfo is null");
            }
            else
            {
                lblSuccessTotalAmount.Text = FormMain.ClientInfo.CashCodeAmount + @" " + FormMain.ClientInfo.CurrentCurrency;
            }

            string productName = String.Empty;
            string accountNumber = String.Empty;
            try
            {
                switch (FormMain.ClientInfo.ProductCode)
                {
                    case 1:
                        productName = "Оплата кредита";
                        accountNumber = FormMain.ClientInfo.CreditAccountNumber;
                        break;

                    case 2:
                        productName = "Пополнение счета";
                        accountNumber = FormMain.ClientInfo.AccountNumber;
                        break;
                }

                Log.Debug("Update print info");
                _StreamToPrint = String.Format(PRINT_TEMPLATE,
                    DateTime.Now,
                    FormMain.TerminalInfo != null ? FormMain.TerminalInfo.Id.ToString(CultureInfo.InvariantCulture) : @"[NULL]",                    
                    FormMain.ClientInfo != null ? FormMain.ClientInfo.PaymentId.ToString(CultureInfo.InvariantCulture) : @"[NULL]",
                    FormMain.ClientInfo != null ? FormMain.ClientInfo.TransactionId.ToString(CultureInfo.InvariantCulture) : @"[NULL]",
                                               productName,
                                               accountNumber,
                                               lblSuccessTotalAmount.Text);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            try
            {
                var printThread = new Thread(new ThreadStart(PrintAsync));
                printThread.Start();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
            Log.Debug("End");
        }

        private void PrintAsync()
        {
            try
            {
                Log.Debug("Print");
                printDocument.PrintController = new StandardPrintController();
                printDocument.Print();
                Log.Debug("Print end");
                printDocument.EndPrint += PrintDocumentOnEndPrint;
            }
            catch (Exception ex)
            {
                Log.ErrorException(ex.Message, ex);
            }

            Log.Debug("End");
        }        

        private void PrintDocumentOnEndPrint(object sender, PrintEventArgs printEventArgs)
        {
            Log.Info(String.Format("Printed. {0}", printEventArgs.PrintAction));
        }

        private void PrintDocumentPrintPage(object sender, PrintPageEventArgs ev)
        {
            try
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
                float linesPerPage = ev.MarginBounds.Height/
                                     messageFont.GetHeight(ev.Graphics);

                // Print each line of the file.
                yPos = topMargin;
                ev.Graphics.DrawString(_StreamToPrint, messageFont, Brushes.Black,
                                       leftMargin, yPos, new StringFormat());
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }
    }
}
