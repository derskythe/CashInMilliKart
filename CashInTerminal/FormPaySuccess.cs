using System;
using System.Drawing;

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
            "Операция: {3}\n" +
            "Номер счета: {4}\n" +
            "Сумма: {5}\n" +
            "-------------------------------------------------\n" +
            "\n" +
            "\n" +
            "Спасибо\n\n\n\n";

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
            //base.OnLoad(e);
            lblSuccessTotalAmount.Text = FormMain.ClientInfo.CashCodeAmount + @" " + FormMain.ClientInfo.CurrentCurrency;

            string productName = String.Empty;
            string accountNumber = String.Empty;

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

            _StreamToPrint = String.Format(PRINT_TEMPLATE, DateTime.Now, FormMain.TerminalInfo.Id,
                                           FormMain.ClientInfo.PaymentId, productName, accountNumber,
                                           lblSuccessTotalAmount.Text);

            printDocument.Print();
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
