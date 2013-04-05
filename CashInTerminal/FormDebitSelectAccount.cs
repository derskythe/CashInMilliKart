using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CashInTerminal.BaseForms;
using Containers.Enums;

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
            switch (FormMain.ClientInfo.PaymentOperationType)
            {
                case PaymentOperationType.DebitPaymentByClientCode:
                    ChangeView(typeof(FormCreditByClientCode));
                    break;

                case PaymentOperationType.DebitPaymentByPassportAndAccount:
                    ChangeView(typeof(FormCreditByPassport1));
                    break;

                default:
                    ChangeView(typeof(FormProducts));
                    break;
            }                        
        }

        private void BtnNextClick(object sender, EventArgs e)
        {
            if (!GetSelectedRow())
            {
                Log.Error("Couldn't find selected value!!!");
                ChangeView(typeof (FormOutOfOrder));
            }
        }

        private void FormDebitSelectAccountLoad(object sender, EventArgs e)
        {
            var buffer = new object[4];
            var rows = new List<DataGridViewRow>();

            foreach (var info in FormMain.Clients)
            {
                buffer[0] = info.ClientAccount;
                buffer[1] = info.CreditName;
                buffer[2] = info.BeginDate.ToString("dd MMMM yyyy");
                buffer[3] = info.Currency;
                rows.Add(new DataGridViewRow());
                rows[rows.Count - 1].CreateCells(dataGridSelect, buffer);
            }

            dataGridSelect.Rows.AddRange(rows.ToArray());
            ResizeDataGrid(dataGridSelect);
        }

        private void DataGridSelectCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            GetSelectedRow();
        }

        private bool GetSelectedRow()
        {
            foreach (DataGridViewRow row in dataGridSelect.SelectedRows)
            {
                foreach (var info in FormMain.Clients)
                {
                    if (info.ClientAccount == row.Cells[0].Value.ToString())
                    {
                        FormMain.ClientInfo.Client = info;
                        FormMain.ClientInfo.CurrentCurrency = info.Currency;
                        ChangeView(typeof (FormCreditClientInfo));
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
