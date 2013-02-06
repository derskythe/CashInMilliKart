using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using CashInTerminal.Enums;
using Org.BouncyCastle.Security;

namespace CashInTerminal
{
    public partial class FormDebitSelectAccount : FormMdiChild
    {
        public FormDebitSelectAccount()
        {
            InitializeComponent();
        }

        private void BtnBackClick(object sender, EventArgs e)
        {
            if (FormMain.ClientInfo.DebitPayType == DebitPayType.ByCardFull)
            {
                ChangeView(new FormDebitCardFull());
            }
            else
            {
                ChangeView(new FormClientCode());
            }
        }

        private void BtnNextClick(object sender, EventArgs e)
        {
            ChangeView(new FormDebitClientInfo());
        }

        private void FormDebitSelectAccountLoad(object sender, EventArgs e)
        {
            //base.OnLoad(e);

            var buffer = new object[4];
            var rows = new List<DataGridViewRow>();
            Random rnd = new SecureRandom();

            for (int i = 0; i <= 10; i++)
            {
                buffer[0] = rnd.Next().ToString(CultureInfo.InvariantCulture);
                buffer[1] = "Карточный";
                buffer[2] = DateTime.Now.AddMonths(-3).ToShortDateString();
                buffer[3] = "AZN";

                rows.Add(new DataGridViewRow());
                rows[rows.Count - 1].CreateCells(dataGridSelect, buffer);
            }

            dataGridSelect.Rows.AddRange(rows.ToArray());
        }

        private void DataGridSelectCellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridSelect.SelectedRows)
            {
                FormMain.ClientInfo.CreditAccountNumber = row.Cells[0].Value.ToString();
                FormMain.ClientInfo.CurrentCurrency = row.Cells[3].Value.ToString();
                break;
            }
        }        
    }
}
