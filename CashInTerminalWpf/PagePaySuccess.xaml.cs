using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using CashInTerminal.Enums;
using CashInTerminalWpf.Enums;
using CashInTerminalWpf.Properties;
using Containers;
using NLog;
using CheckField = CashInTerminalWpf.CashIn.CheckField;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PagePaySuccess.xaml
    /// </summary>
    public partial class PagePaySuccess
    {
        private MainWindow _FormMain;
        private readonly PrintDocument _PrintDocument = new PrintDocument();
        private String _DateNow;
        private string _ProductName;
        private List<CheckField> _Template;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local


        public PagePaySuccess()
        {
            InitializeComponent();            
        }

        private void PrintDocumentOnPrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
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

        private void PrintDocumentOnEndPrint(object sender, PrintEventArgs printEventArgs)
        {
            Log.Info(String.Format("Printed. {0}", printEventArgs.PrintAction));
        }

        private void ButtonSuccessClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.Products);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Name);

            _FormMain = (MainWindow)Window.GetWindow(this);

            _PrintDocument.PrintPage += PrintDocumentOnPrintPage;
            _PrintDocument.EndPrint += PrintDocumentOnEndPrint;

            try
            {
                Log.Info(_FormMain.ClientInfo);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }


            if (_FormMain.ClientInfo.CurrentCurrency != _FormMain.ClientInfo.Client.Currency)
            {
                LabelCommission.Visibility = Visibility.Visible;
            }
            //base.OnLoad(e);
            if (_FormMain.ClientInfo == null)
            {
                Log.Error("ClientInfo is null");
            }
            else
            {
                LabelAmount.Content = _FormMain.ClientInfo.CashCodeAmount + @" " + _FormMain.ClientInfo.CurrentCurrency;
            }

            try
            {
                _ProductName = _FormMain.ClientInfo != null ? GetProductName(Convert.ToInt32(_FormMain.ClientInfo.Product.Id)) : @"[NULL]";
                _DateNow = Utilities.FormatDate(DateTime.Now);

                lock (_FormMain.CheckTemplates)
                {
                    _Template = new List<CheckField>();
                    List<ds.TemplateFieldRow> rows;
                    _FormMain.CheckTemplates.TryGetValue(
                        _FormMain.GetCheckTemplateHashCode(_FormMain.ClientInfo.Product.CheckType,
                                                          _FormMain.SelectedLanguage), out rows);


                    if (rows != null)
                    {
                        Log.Debug(EnumEx.GetStringFromArray(rows));

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
                    else
                    {
                        Log.Warn("Rows is null " + _FormMain.ClientInfo.Product.CheckType);

                        Log.Warn(_FormMain.GetCheckTemplateHashCode(_FormMain.ClientInfo.Product.CheckType,
                                                          _FormMain.SelectedLanguage));

                        foreach (KeyValuePair<int, List<ds.TemplateFieldRow>> pair in _FormMain.CheckTemplates)
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
            foreach (var item in _FormMain.Products)
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
                _PrintDocument.PrintController = new StandardPrintController();
                _PrintDocument.Print();
                Log.Debug("Print end");
                _PrintDocument.EndPrint += PrintDocumentOnEndPrint;
            }
            catch (Exception ex)
            {
                Log.ErrorException(ex.Message, ex);
            }

            Log.Debug("End");
        }

        private string ReplaceTemplateFields(String value)
        {
            value = value.Replace(TemplateFields.Amount, _FormMain.ClientInfo.CashCodeAmount.ToString(CultureInfo.InvariantCulture));
            value = value.Replace(TemplateFields.ClientAccount, _FormMain.ClientInfo.Client.ClientAccount);
            value = value.Replace(TemplateFields.ClientCode, _FormMain.ClientInfo.Client.ClientCode);
            value = value.Replace(TemplateFields.Currency, _FormMain.ClientInfo.CurrentCurrency);
            value = value.Replace(TemplateFields.DateTime, _DateNow);
            value = value.Replace(TemplateFields.OperationCode, _FormMain.ClientInfo != null ? _FormMain.ClientInfo.PaymentId.ToString(CultureInfo.InvariantCulture) : @"[NULL]");
            value = value.Replace(TemplateFields.ProductName, _ProductName);
            value = value.Replace(TemplateFields.TransactionId, _FormMain.ClientInfo != null ? _FormMain.ClientInfo.TransactionId.ToString(CultureInfo.InvariantCulture) : @"[NULL]");
            if (_FormMain.TerminalInfo != null)
            {
                value = value.Replace(TemplateFields.Branch, _FormMain.TerminalInfo.BranchName);
                value = value.Replace(TemplateFields.TerminalId, _FormMain.TerminalInfo.Id.ToString(CultureInfo.InvariantCulture));
                value = value.Replace(TemplateFields.Address, _FormMain.TerminalInfo.Address);
            }
            value = value.Replace(TemplateFields.TransactionId, _FormMain.ClientInfo != null ? _FormMain.ClientInfo.TransactionId.ToString(CultureInfo.InvariantCulture) : @"[NULL]");
            value = value.Replace(TemplateFields.FullPaymentFlag, _FormMain.ClientInfo != null && _FormMain.ClientInfo.Client.AmountLeft > _FormMain.ClientInfo.CashCodeAmount ? Properties.Resources.CreditOpen : Properties.Resources.CreditClosed);

            return value;
        }
    }
}
