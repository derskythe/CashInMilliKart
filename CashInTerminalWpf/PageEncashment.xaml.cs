using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using CashInTerminal.Enums;
using CashInTerminalWpf.CashIn;
using CashInTerminalWpf.Enums;
using Containers.Enums;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageEncashment.xaml
    /// </summary>
    public partial class PageEncashment
    {
        private readonly PrintDocument _PrintDocument = new PrintDocument();
        private MainWindow _FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        private List<CheckField> _Template;
        private String _DateNow;
        private String _Amount;
        private String _BillsCount;

        public PageEncashment()
        {
            InitializeComponent();
        }

        private void PrintDocumentOnPrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                Log.Info("Printing check!");
                using (var font = new Font("Arial", 10, System.Drawing.FontStyle.Bold))
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

                                        int width = img.Width;
                                        if (width > e.PageBounds.Width - 15)
                                        {
                                            width = e.PageBounds.Width - 15;
                                        }

                                        int height = img.Height;
                                        if (height > e.PageBounds.Height)
                                        {
                                            height = e.PageBounds.Height;
                                        }

                                        var convertedImage = Utilities.ScaleImage(img, width, height);

                                        var sz = new SizeF(100 * convertedImage.Width / convertedImage.HorizontalResolution,
                                                           100 * convertedImage.Height / convertedImage.VerticalResolution);
                                        var p = new PointF((e.PageBounds.Width - sz.Width) / 2, y);

                                        e.Graphics.DrawImage(convertedImage, p);
                                        y += sz.Height + lineHeight;
                                        img.Dispose();
                                        stream.Close();
                                    }
                                }
                                catch (Exception exp)
                                {
                                    Log.ErrorException(exp.Message, exp);
                                }
                                Log.Info("IMAGE");
                                break;

                            case TemplateFieldType.Line:
                                using (var blackPen = new Pen(Color.Black, 3))
                                {
                                    y += lineHeight;
                                    e.Graphics.DrawLine(blackPen, 0, y, e.PageBounds.Width, y);
                                    y += lineHeight;
                                    blackPen.Dispose();
                                }
                                Log.Info("-------------------------");
                                break;

                            case TemplateFieldType.Text:
                                var subLines = line.Value.Split('\n');
                                foreach (var subLine in subLines)
                                {
                                    Log.Info(subLine);
                                    e.Graphics.DrawString(subLine, font, Brushes.Black, x, y);
                                    y += lineHeight;
                                }                                
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

            //try
            //{
            //    Settings.Default.CheckCounter++;
            //    Settings.Default.Save();
            //}
            //catch (Exception exp)
            //{
            //    Log.ErrorException(exp.Message, exp);
            //}
        }

        private void ButtonEncashmentPrintClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _DateNow = Utilities.FormatDate(DateTime.Now);
                var msg = new StringBuilder();
                foreach (var currency in _FormMain.Currencies)
                {
                    var total = _FormMain.Db.GetCasseteTotal(currency.Name);
                    msg.Append(total).Append(" ").Append(currency.Name).Append("\n");
                }

                _Amount = msg.ToString();

                msg = new StringBuilder();

                try
                {
                    var listBills = _FormMain.Db.CountAllBanknotes();
                    foreach (var row in listBills)
                    {
                        msg.Append(row.CountAll)
                           .Append("\tX\t")
                           .Append(row.Amount)
                           .Append(" ")
                           .Append(row.Currency)
                           .Append("\n");
                    }
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                }

                _BillsCount = msg.ToString();

                //_StreamToPrint = String.Format(PRINT_TEMPLATE, DateTime.Now, FormMain.TerminalInfo.Id, msg);

                //Log.Info(msg.ToString());

                lock (_FormMain.CheckTemplates)
                {
                    _Template = new List<CheckField>();
                    List<ds.TemplateFieldRow> rows;
                    _FormMain.CheckTemplates.TryGetValue(
                        _FormMain.GetCheckTemplateHashCode((int)CheckTemplateTypes.EncashmentFull,
                                                          _FormMain.SelectedLanguage), out rows);
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

                _PrintDocument.PrintController = new StandardPrintController();
                _PrintDocument.Print();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Title);
            _FormMain = (MainWindow)Window.GetWindow(this);
            _PrintDocument.PrintPage += PrintDocumentOnPrintPage;
        }

        private void ButtonEncashmentFinishClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _FormMain.Db.DeleteCasseteBanknotes();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            Thread.Sleep(1000);

            try
            {
                _FormMain.CcnetDevice.Reset();
            }
            catch (Exception exp) 
            {
                Log.ErrorException(exp.Message, exp);
            }

            _FormMain.TerminalStatus = TerminalCodes.Ok;
            _FormMain.OpenForm(FormEnum.Products);
        }

        private string ReplaceTemplateFields(String value)
        {
            value = value.Replace(TemplateFields.Amount, _Amount);
            value = value.Replace(TemplateFields.BillsCount, _BillsCount);
            value = value.Replace(TemplateFields.ClientAccount, String.Empty);
            value = value.Replace(TemplateFields.ClientCode, String.Empty);
            value = value.Replace(TemplateFields.Currency, String.Empty);
            value = value.Replace(TemplateFields.DateTime, _DateNow);
            value = value.Replace(TemplateFields.OperationCode, String.Empty);
            value = value.Replace(TemplateFields.ProductName, String.Empty);
            if (_FormMain.TerminalInfo != null)
            {
                value = value.Replace(TemplateFields.Branch, _FormMain.TerminalInfo.BranchName);
                value = value.Replace(TemplateFields.TerminalId, _FormMain.TerminalInfo.Id.ToString(CultureInfo.InvariantCulture));
                value = value.Replace(TemplateFields.Address, _FormMain.TerminalInfo.Address);
            }
            value = value.Replace(TemplateFields.TransactionId, String.Empty);

            return value;
        }
    }
}
