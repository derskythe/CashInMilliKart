using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using CashInTerminal.BaseForms;
using Containers.Enums;

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
            switch (FormMain.ClientInfo.PaymentOperationType)
            {
                case PaymentOperationType.CreditPaymentByClientCode:
                    ChangeView(typeof(FormCreditByClientCode));
                    break;

                case PaymentOperationType.CreditPaymentByPassportAndAccount:
                    ChangeView(typeof(FormCreditByPassport1));
                    break;

                case PaymentOperationType.CreditPaymentBolcard:
                    ChangeView(typeof(FormCreditByBolcard));
                    break;

                default:
                    ChangeView(typeof(FormProducts));
                    break;
            }            
        }

        //private void BtnNextClick(object sender, EventArgs e)
        //{
        //    if (!GetSelectedRow())
        //    {
        //        Log.Error("Couldn't find selected value!!!");
        //        ChangeView(typeof (FormOutOfOrder));
        //    }
        //}

        private void FormCreditSelectAccountLoad(object sender, EventArgs e)
        {
            //var buffer = new object[5];
            //var rows = new List<DataGridViewRow>();

            //foreach (var info in FormMain.Clients)
            //{
            //    buffer[0] = info.ClientAccount;
            //    buffer[1] = info.CreditName;
            //    buffer[2] = info.BeginDate.ToString("dd MMMM yyyy");
            //    buffer[3] = info.CreditAmount;
            //    buffer[4] = info.Currency;
            //    rows.Add(new DataGridViewRow());
            //    rows[rows.Count - 1].CreateCells(dataGridSelect, buffer);
            //}

            //dataGridSelect.Rows.AddRange(rows.ToArray());
            //ResizeDataGrid(dataGridSelect);
            try
            {
                if (FormMain.Clients != null && FormMain.Clients.Length > 0)
                {
                    AddButtons();
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        //private void DataGridSelectCellClick(object sender, DataGridViewCellEventArgs e)
        //{
            
        //}

        //private void DataGridSelectCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    GetSelectedRow();
        //}

        //private bool GetSelectedRow()
        //{
        //    foreach (DataGridViewRow row in dataGridSelect.SelectedRows)
        //    {
        //        //Log.Debug(row.Cells[0].Value.ToString());
        //        foreach (var info in FormMain.Clients)
        //        {
        //            if (info.ClientAccount == row.Cells[0].Value.ToString())
        //            {
        //                FormMain.ClientInfo.Client = info;
        //                FormMain.ClientInfo.CurrentCurrency = info.Currency;
        //                ChangeView(typeof (FormCreditClientInfo));
        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        private void AddButtons()
        {
            lock (FormMain.Clients)
            {
                tableLayoutPanel.Controls.Clear();
                tableLayoutPanel.RowCount = 0;

                try
                {
                    using (var font = new Font("Microsoft Sans Serif", 27.75F, FontStyle.Bold))
                    {
                        foreach (var tag in FormMain.Clients)
                        {
                            //    buffer[0] = info.ClientAccount;
                            //    buffer[1] = info.CreditName;
                            //    buffer[2] = info.BeginDate.ToString("dd MMMM yyyy");
                            //    buffer[3] = info.CreditAmount;
                            //    buffer[4] = info.Currency;

                            string text = tag.ClientAccount + @" " + tag.CreditName + " " +
                                          tag.BeginDate.ToString("MM.yyyy") + " " + tag.CreditAmount + " " +
                                          tag.Currency;
                            Log.Debug(text);

                            var button = new Button
                            {
                                Size = new Size(996, 105),
                                Font = font,
                                BackColor = Color.Transparent,
                                Name = tag.ClientAccount.ToString(CultureInfo.InvariantCulture) + tag.CreditAmount,
                                Tag = tag,
                                Text = text
                            };
                            button.Click += ButtonOnClick;

                            if (tableLayoutPanel.Controls.Count > 0)
                            {
                                var rowNum = AddTableRow();
                                Log.Debug(String.Format("Text: {0}, RowNum: {1}", text, rowNum));
                                tableLayoutPanel.Controls.Add(button, 0, rowNum);
                            }
                            else
                            {
                                var rowNum = AddTableRow();
                                Log.Debug(String.Format("Text: {0}, RowNum: {1}", text, rowNum));
                                tableLayoutPanel.Controls.Add(button, 0, rowNum);
                            }
                        }

                        tableLayoutPanel.AutoSize = true;
                    }
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                }
            }
        }

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            try
            {
                var button = (Button)sender;
                FormMain.ClientInfo.Client = (CashIn.ClientInfo)button.Tag;
                ChangeView(typeof (FormCreditClientInfo));
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private int AddTableRow()
        {
            int index = tableLayoutPanel.RowCount++;
            var style = new RowStyle(SizeType.AutoSize);
            tableLayoutPanel.RowStyles.Add(style);
            return index;
        }
    }
}
