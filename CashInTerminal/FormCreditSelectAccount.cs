using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CashInTerminal.CashIn;

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
            switch (FormMain.ClientInfo.GetClientInfoType)
            {
                case GetClientInfoType.ByClientCode:
                    ChangeView(typeof(FormCreditByClientCode));
                    break;


            }            
        }

        private void BtnNextClick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridSelect.SelectedRows)
            {
                //Log.Debug(row.Cells[0].Value.ToString());
                foreach (var info in FormMain.Clients)
                {
                    if (info.ClientAccount == row.Cells[0].Value.ToString())
                    {
                        FormMain.ClientInfo.Client = info;
                        FormMain.ClientInfo.CurrentCurrency = info.Currency;
                        ChangeView(typeof(FormCreditClientInfo));
                        return;
                    }
                }
            }

            Log.Error("Couldn't find selected value!!!");
            ChangeView(typeof(FormOutOfOrder));
        }

        private void FormCreditSelectAccountLoad(object sender, EventArgs e)
        {
            var buffer = new object[5];
            var rows = new List<DataGridViewRow>();

            foreach (var info in FormMain.Clients)
            {
                buffer[0] = info.ClientAccount;
                buffer[1] = info.CreditName;
                buffer[2] = info.BeginDate.ToString("dd MMMM yyyy");
                buffer[3] = info.CreditAmount;
                buffer[4] = info.Currency;
                rows.Add(new DataGridViewRow());
                rows[rows.Count - 1].CreateCells(dataGridSelect, buffer);
            }

            dataGridSelect.Rows.AddRange(rows.ToArray());
            ResizeDataGrid(dataGridSelect);
        }

        private void DataGridSelectCellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}
