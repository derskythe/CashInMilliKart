using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using LogAnalizer.Properties;

namespace LogAnalizer
{
    public partial class FormMain : Form
    {
        private const String EOL = "\r\n";
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMainLoad(object sender, EventArgs e)
        {
            cmbTransactions.SelectedIndex = Settings.Default.CountType;
        }

        private void BtnOpenClick(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Settings.Default.InitDir))
            {
                Settings.Default.InitDir = Environment.SpecialFolder.MyComputer.ToString();
            }

            openFileDialog.InitialDirectory = Settings.Default.InitDir;
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.InitDir = Path.GetDirectoryName(openFileDialog.FileName);
                Settings.Default.CountType = cmbTransactions.SelectedIndex;
                Settings.Default.Save();

                string fileContent;
                using (var file = new StreamReader(openFileDialog.FileName, Encoding.UTF8))
                {
                    fileContent = file.ReadToEnd();
                    file.Close();
                }

                var splited = fileContent.Split('\n');

                var transId = new Regex(@"Starting\s+transId\:\s+(\d+)", RegexOptions.IgnoreCase);
                var regAzn = new Regex(@"(\d+)\s+(AZN)", RegexOptions.IgnoreCase);
                var regUsd = new Regex(@"(\d+)\s+(USD)", RegexOptions.IgnoreCase);
                var regDate = new Regex(@"^(\d+\-\d+)(\s+\d+\:\d+\:\d+\.\d+)", RegexOptions.IgnoreCase);
                const string format = "dd-MM-yyyy HH:mm:ss.fff";
                //DateTime date = DateTime.ParseExact(str, format, CultureInfo.InvariantCulture);
                int amountAzn = 0;
                int amountUsd = 0;
                int totalAmountAzn = 0;
                int totalAmountUsd = 0;
                DateTime prevDate = DateTime.MinValue;
                txtTrasnsactions.Text = String.Empty;
                txtTotal.Text = String.Empty;
                txtWarning.Text = String.Empty;
                txtWarning2.Text = String.Empty;
                var totalTrans = new Dictionary<String, int>();
                var sql = new StringBuilder();
                sql.Append("SELECT * FROM products_history t WHERE ");
                var transactionList = new List<string>();
                String prevTransaction = String.Empty;

                foreach (var line in splited)
                {
                    if (line.IndexOf("Starting transId", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        if (amountAzn > 0)
                        {
                            txtTrasnsactions.Text += @"Total amount: " + amountAzn + @" AZN" + EOL;
                            

                            if (cmbTransactions.SelectedIndex == 0)
                            {
                                var matches = transId.Matches(prevTransaction);
                                if (matches.Count == 1)
                                {
                                    String transIdRaw = matches[0].Groups[1].ToString();
                                    totalTrans.Add(transIdRaw, amountAzn);
                                    transactionList.Add("t.transaction_id = '" + transIdRaw + "'");
                                }
                            }
                        }
                        else if (amountUsd > 0)
                        {
                            txtTrasnsactions.Text += @"Total amount: " + amountUsd + @" USD" + EOL;

                            if (cmbTransactions.SelectedIndex == 1)
                            {
                                var matches = transId.Matches(prevTransaction);
                                if (matches.Count == 1)
                                {
                                    String transIdRaw = matches[0].Groups[1].ToString();
                                    totalTrans.Add(transIdRaw, amountUsd);
                                    transactionList.Add("t.transaction_id = '" + transIdRaw + "'");
                                }
                            }
                        }
                        txtTrasnsactions.Text += line.TrimEnd() + EOL;
                        totalAmountAzn += amountAzn;
                        totalAmountUsd += amountUsd;
                        amountAzn = 0;
                        amountUsd = 0;
                        prevTransaction = line;
                    }
                    else if (line.IndexOf("[INFO ] CashInTerminalWpf.PageMoneyInput.CcnetDeviceOnBillStacked - Stacked", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        var matches = regAzn.Matches(line);
                        if (matches.Count == 1)
                        {
                            amountAzn += Convert.ToInt32(matches[0].Groups[1].ToString());
                            txtTrasnsactions.Text += matches[0].Groups[1] + @" AZN" + EOL;
                        }

                        matches = regUsd.Matches(line);
                        if (matches.Count == 1)
                        {
                            amountUsd += Convert.ToInt32(matches[0].Groups[1].ToString());
                            txtTrasnsactions.Text += matches[0].Groups[1] + @" USD" + EOL;
                        }

                        matches = regDate.Matches(line);

                        if (matches.Count == 1)
                        {
                            String date = matches[0].Groups[1] + "-" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture) +
                                          matches[0].Groups[2];
                            //if (date == "29-05-2013 15:16:09.683")
                            //{
                            //    var i = 0;
                            //}
                            DateTime nextDate = DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);

                            if (nextDate - prevDate < new TimeSpan(0, 0, 0, 5))
                            {
                                txtWarning2.Text += line.TrimEnd() + EOL;
                            }
                            prevDate = nextDate;
                        }
                    }
                    else if (line.IndexOf("CassetteRemoved", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        txtTrasnsactions.Text += line.TrimEnd() + EOL;
                        txtWarning.Text += line.TrimEnd() + EOL;
                    }
                    else if (
                        line.IndexOf("CashInTerminalWpf.MainWindow.DoEncashment",
                                     StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        txtTrasnsactions.Text += line.TrimEnd() + EOL;
                        txtWarning.Text += line.TrimEnd() + EOL;
                    }
                }

                if (amountAzn > 0)
                {
                    txtTrasnsactions.Text += @"Total amount: " + amountAzn + @" AZN" + EOL;
                    totalAmountAzn += amountAzn;

                    if (cmbTransactions.SelectedIndex == 0)
                    {
                        var matches = transId.Matches(prevTransaction);

                        if (matches.Count == 1)
                        {
                            String transIdRaw = matches[0].Groups[1].ToString();
                            totalTrans.Add(transIdRaw, amountAzn);
                            transactionList.Add("t.transaction_id = '" + transIdRaw + "'");
                        }
                    }
                }
                else if (amountUsd > 0)
                {
                    txtTrasnsactions.Text += @"Total amount: " + amountUsd + @" USD" + EOL;
                    totalAmountUsd += amountUsd;

                    if (cmbTransactions.SelectedIndex == 1)
                    {
                        var matches = transId.Matches(prevTransaction);

                        if (matches.Count == 1)
                        {
                            String transIdRaw = matches[0].Groups[1].ToString();
                            totalTrans.Add(transIdRaw, amountUsd);
                            transactionList.Add("t.transaction_id = '" + transIdRaw + "'");
                        }
                    }
                }

                sql.Append(String.Join(" OR ", transactionList)).Append(EOL);

                foreach (var tran in totalTrans)
                {
                    sql.Append("TransactionID: ").Append(tran.Key).Append(" ").Append("Amount: ").Append(tran.Value).Append(EOL);
                }


                txtWarning2.Text += sql.ToString();
                txtTotal.Text = totalAmountAzn.ToString(CultureInfo.InvariantCulture) + @" AZN, " +
                                totalAmountUsd.ToString(CultureInfo.InvariantCulture) + @" USD";
            }
        }
    }
}
