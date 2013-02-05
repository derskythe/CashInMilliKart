using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using Org.BouncyCastle.Security;

namespace CashInTerminal
{
    public partial class FormCreditSelectAccount : FormMdiChild
    {
        public FormCreditSelectAccount()
        {
            InitializeComponent();
        }

        private void BtnBackClick(object sender, EventArgs e)
        {
            ChangeView(new FormCreditClientCode());
        }

        private void BtnNextClick(object sender, EventArgs e)
        {
            ChangeView(new FormCreditClientInfo());
        }

        private void FormCreditSelectAccountLoad(object sender, EventArgs e)
        {
            base.OnLoad(e);

            var buffer = new object[5];
            var rows = new List<DataGridViewRow>();
            Random rnd = new SecureRandom();

            for (int i = 0; i <= 10; i++)
            {
                buffer[0] = rnd.Next().ToString(CultureInfo.InvariantCulture);
                buffer[1] = "Потребительский";
                buffer[2] = DateTime.Now.AddMonths(-3).ToShortDateString();
                buffer[3] = "1000";
                buffer[4] = "AZN";

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
                FormMain.ClientInfo.CurrentCurrency = row.Cells[4].Value.ToString();
                break;
            }
        }
    }
}
