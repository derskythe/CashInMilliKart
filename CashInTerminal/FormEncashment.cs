using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CashInTerminal.CashIn;
using CashInTerminal.Enums;
using Containers.Enums;

namespace CashInTerminal
{
    public partial class FormEncashment : FormMdiChild
    {
        //String _StreamToPrint = String.Empty;

        //private const string PRINT_TEMPLATE =
        //    "    Инкассация терминала\n" + 
        //    "\n" + 
        //    "Дата: {0:t} {0:d}\n" +
        //    "-------------------------------------------------\n" + 
        //    "№ терминала: {1}\n" + 
        //    "{2}\n" +
        //    "-------------------------------------------------\n" + 
        //    "\n\n\n\n\n\n";

        private List<CheckField> _Template;
        private String _DateNow;
        private String _Amount;

        public FormEncashment()
        {
            InitializeComponent();
        }

        private void BtnEncashmentPrintClick(object sender, EventArgs e)
        {
            try
            {
                _DateNow = Utilities.FormatDate(DateTime.Now);
                var msg = new StringBuilder();
                foreach (var currency in FormMain.Currencies)
                {
                    var total = FormMain.Db.GetCasseteTotal(currency.Name);
                    msg.Append(total).Append(" ").Append(currency.Name).Append("\n");
                }

                _Amount = msg.ToString();

                //_StreamToPrint = String.Format(PRINT_TEMPLATE, DateTime.Now, FormMain.TerminalInfo.Id, msg);

                //Log.Info(msg.ToString());

                lock (FormMain.CheckTemplates)
                {
                    _Template = new List<CheckField>();
                    List<ds.TemplateFieldRow> rows;
                    FormMain.CheckTemplates.TryGetValue(
                        FormMain.GetCheckTemplateHashCode((int)CheckTemplateTypes.Encashment,
                                                          FormMain.SelectedLanguage), out rows);
                    if (rows != null)
                    {
                        foreach (var row in rows)
                        {
                            var field = new CheckField
                            {
                                Id = (int)row.Id,
                                CheckId = (int)row.CheckTemplateId,
                                FieldType = (int)row.Type,
                                Image = row.IsImageNull() ? null : row.Image,
                                Order = row.IsOrderNumberNull() ? 0 : (int)row.OrderNumber,
                                Value = row.IsValueNull() ? String.Empty : ReplaceTemplateFields(row.Value)
                            };

                            _Template.Add(field);
                        }
                    }
                }

                printDocument.PrintController = new StandardPrintController();
                printDocument.Print();
                //MessageBox.Show(msg.ToString());
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
            FormMain.TerminalStatus = TerminalCodes.Ok;

            Cursor.Current = Cursors.Default;
            ChangeView(typeof(FormProducts));
        }

        private void PrintDocumentPrintPage(object sender, PrintPageEventArgs e)
        {
            using (var font = new Font("Arial", 10, FontStyle.Bold))
            {
                const float x = 0;
                float y = 0;

                // Determine the height of a line (based on the font used).
                float lineHeight = font.GetHeight(e.Graphics);
                y += lineHeight * 2;

                foreach (var line in _Template)
                {
                    switch ((TemplateFieldType)line.FieldType)
                    {
                        case TemplateFieldType.Image:
                            try
                            {
                                using (var stream = new MemoryStream(line.Image))
                                {
                                    Image img = new Bitmap(stream);
                                    var convertedImage = Utilities.ScaleImage(img, e.PageBounds.Width - 15, e.PageBounds.Height);
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

        private string ReplaceTemplateFields(String value)
        {
            value = value.Replace(TemplateFields.Amount, _Amount);
            value = value.Replace(TemplateFields.ClientAccount, String.Empty);
            value = value.Replace(TemplateFields.ClientCode, String.Empty);
            value = value.Replace(TemplateFields.Currency, String.Empty);
            value = value.Replace(TemplateFields.DateTime, _DateNow);
            value = value.Replace(TemplateFields.OperationCode, String.Empty);
            value = value.Replace(TemplateFields.ProductName, String.Empty);
            value = value.Replace(TemplateFields.TerminalId, FormMain.TerminalInfo != null ? FormMain.TerminalInfo.Id.ToString(CultureInfo.InvariantCulture) : @"[NULL]");
            value = value.Replace(TemplateFields.TransactionId, String.Empty);

            return value;
        }
    }
}
