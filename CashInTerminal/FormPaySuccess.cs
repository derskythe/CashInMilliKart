using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Threading;
using CashInTerminal.BaseForms;
using CashInTerminal.Enums;
using CashInTerminal.Properties;
using Containers;
using CheckField = CashInTerminal.CashIn.CheckField;

namespace CashInTerminal
{
    public partial class FormPaySuccess : FormMdiChild
    {
        //readonly String _StreamToPrint = String.Empty;
        private string _ProductName;
        //private const string PRINT_TEMPLATE =
        //    "    Оплата посредством терминала\n" +
        //    "\n" +
        //    "Дата: {0:t} {0:d}\n" +
        //    "-------------------------------------------------\n" +
        //    "№ терминала: {1}\n" +
        //    "Код: {2}\n" +
        //    "Код транзакции: {3}\n" +
        //    "Операция: {4}\n" +
        //    "Номер счета: {5}\n" +
        //    "Сумма: {6}\n" +
        //    "-------------------------------------------------\n" +
        //    "\n" +
        //    "\n" +
        //    "Спасибо\n\n\n\n";

        //private const string PRINT_TEMPLATE2 = "    Оплата посредством терминала\n" +
        //    "\n" +
        //    "Дата: {0:t} {0:d}\n" +
        //    "-------------------------------------------------\n" +
        //    "№ терминала: {1}\n" +
        //    "Код: {2}\n" +
        //    "Код транзакции: {3}\n" +
        //    "Операция: {4}\n" +
        //    "Код клиента: {5}\n" +
        //    "Номер счета: {6}\n" +
        //    "Сумма: {7}\n" +
        //    "-------------------------------------------------\n" +
        //    "\n" +
        //    "\n" +
        //    "Спасибо\n\n\n\n";
        private List<CheckField> _Template;
        public delegate string AsyncMethodCaller();

        private String _DateNow;

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

            if (FormMain.ClientInfo.CurrentCurrency != FormMain.ClientInfo.Client.Currency)
            {
                lblComission.Visible = true;
            }
            //base.OnLoad(e);
            if (FormMain.ClientInfo == null)
            {
                Log.Error("ClientInfo is null");
            }
            else
            {
                lblSuccessTotalAmount.Text = FormMain.ClientInfo.CashCodeAmount + @" " + FormMain.ClientInfo.CurrentCurrency;
            }

            try
            {                
                _ProductName = FormMain.ClientInfo != null ? GetProductName(Convert.ToInt32(FormMain.ClientInfo.Product.Id)) : @"[NULL]";
                _DateNow = Utilities.FormatDate(DateTime.Now);

                lock (FormMain.CheckTemplates)
                {
                    _Template = new List<CheckField>();
                    List<ds.TemplateFieldRow> rows;
                    FormMain.CheckTemplates.TryGetValue(
                        FormMain.GetCheckTemplateHashCode(FormMain.ClientInfo.Product.CheckType,
                                                          FormMain.SelectedLanguage), out rows);

                    
                    if (rows != null)
                    {
                        Log.Debug(EnumEx.GetStringFromArray(rows));

                        foreach (var row in rows)
                        {
                            var field = new CheckField
                                {
                                    Id = (int) row.Id,
                                    CheckId = (int) row.CheckTemplateId,
                                    FieldType = (int) row.Type,
                                    Image = row.IsImageNull() ? null : row.Image,
                                    Order = row.IsOrderNumberNull() ? 0 : (int) row.OrderNumber,
                                    Value = row.IsValueNull() ? String.Empty : ReplaceTemplateFields(row.Value)
                                };

                            _Template.Add(field);
                        }
                    }
                    else
                    {
                        Log.Warn("Rows is null " + FormMain.ClientInfo.Product.CheckType);

                        Log.Warn(FormMain.GetCheckTemplateHashCode(FormMain.ClientInfo.Product.CheckType,
                                                          FormMain.SelectedLanguage));

                        foreach (KeyValuePair<int, List<ds.TemplateFieldRow>> pair in FormMain.CheckTemplates)
                        {
                            Log.Debug(String.Format("Key: {0}, Values: {1}", pair.Key, EnumEx.GetStringFromArray(pair.Value)));
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            try
            {
                var printThread = new Thread(PrintAsync);
                printThread.Start();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
            Log.Debug("End");
        }

        private String GetProductName(int id)
        {
            foreach (var item in FormMain.Products)
            {
                if (id == item.Id)
                {
                    if (!String.IsNullOrEmpty(item.NameAz))
                    {
                        return item.NameAz;
                    }
                    if (!String.IsNullOrEmpty(item.NameEn))
                    {
                        return item.NameEn;
                    }

                    return item.NameRu;
                }
            }

            return String.Empty;
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

        private void PrintDocumentPrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                using (var font = new Font("Arial", 10, FontStyle.Bold))
                {
                    const float x = 0;
                    float y = 0;

                    // Determine the height of a line (based on the font used).
                    float lineHeight = font.GetHeight(e.Graphics);
                    y += lineHeight*2;

                    foreach (var line in _Template)
                    {
                        switch ((TemplateFieldType) line.FieldType)
                        {
                            case TemplateFieldType.Image:
                                try
                                {
                                    using (var stream = new MemoryStream(line.Image))
                                    {
                                        Image img = new Bitmap(stream);
                                        var convertedImage = Utilities.ScaleImage(img, e.PageBounds.Width - 15,
                                                                                  e.PageBounds.Height);
                                        e.Graphics.DrawImage(
                                            convertedImage,
                                            x, y);
                                        y += convertedImage.Height + lineHeight;
                                        img.Dispose();
                                        stream.Close();
                                    }
                                }
                                catch (Exception exp)
                                {
                                    Log.ErrorException(exp.Message, exp);
                                }
                                break;

                            case TemplateFieldType.Line:
                                using (var blackPen = new Pen(Color.Black, 3))
                                {
                                    y += lineHeight;
                                    e.Graphics.DrawLine(blackPen, 0, y, e.PageBounds.Width, y);
                                    y += lineHeight;
                                    blackPen.Dispose();
                                }
                                break;

                            case TemplateFieldType.Text:
                                e.Graphics.DrawString(line.Value, font, Brushes.Black, x, y);
                                y += lineHeight;
                                break;
                        }
                    }

                    e.Graphics.DrawString("\n", font, Brushes.Black, x, y);
                    y += lineHeight;
                    e.Graphics.DrawString("\n", font, Brushes.Black, x, y);
                    y += lineHeight;
                    e.Graphics.DrawString("\n", font, Brushes.Black, x, y);

                    font.Dispose();
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            try
            {
                Settings.Default.CheckCounter++;
                Settings.Default.Save();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private string ReplaceTemplateFields(String value)
        {
            value = value.Replace(TemplateFields.Amount, FormMain.ClientInfo.CashCodeAmount.ToString(CultureInfo.InvariantCulture));
            value = value.Replace(TemplateFields.ClientAccount, FormMain.ClientInfo.Client.ClientAccount);
            value = value.Replace(TemplateFields.ClientCode, FormMain.ClientInfo.Client.ClientCode);
            value = value.Replace(TemplateFields.Currency, FormMain.ClientInfo.CurrentCurrency);
            value = value.Replace(TemplateFields.DateTime, _DateNow);
            value = value.Replace(TemplateFields.OperationCode, FormMain.ClientInfo != null ? FormMain.ClientInfo.PaymentId.ToString(CultureInfo.InvariantCulture) : @"[NULL]");
            value = value.Replace(TemplateFields.ProductName, _ProductName);
            value = value.Replace(TemplateFields.TransactionId, FormMain.ClientInfo != null ? FormMain.ClientInfo.TransactionId.ToString(CultureInfo.InvariantCulture) : @"[NULL]");
            if (FormMain.TerminalInfo != null)
            {
                value = value.Replace(TemplateFields.Branch, FormMain.TerminalInfo.BranchName);
                value = value.Replace(TemplateFields.TerminalId, FormMain.TerminalInfo.Id.ToString(CultureInfo.InvariantCulture));
                value = value.Replace(TemplateFields.Address, FormMain.TerminalInfo.Address);
            }
            value = value.Replace(TemplateFields.TransactionId, FormMain.ClientInfo != null ? FormMain.ClientInfo.TransactionId.ToString(CultureInfo.InvariantCulture) : @"[NULL]");
            value = value.Replace(TemplateFields.FullPaymentFlag, FormMain.ClientInfo != null && FormMain.ClientInfo.Client.AmountLeft > FormMain.ClientInfo.CashCodeAmount ? Resources.CreditOpen : Resources.CreditClosed);

            return value;
        }
    }
}
