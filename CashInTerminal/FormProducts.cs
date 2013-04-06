using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using CashInTerminal.BaseForms;
using Containers;
using Product = CashInTerminal.CashIn.Product;
using Timer = System.Threading.Timer;

namespace CashInTerminal
{
    public partial class FormProducts : FormMdiChild
    {
        private delegate void AddControlCallBack();

        private Timer _CheckProductTimer;

        public FormProducts()
        {
            InitializeComponent();
        }

        //private void BtnPayCreditClick(object sender, EventArgs e)
        //{
        //    FormMain.ClientInfo.Product = 1;
        //    ChangeView(typeof(FormCreditTypeSelect));
        //}

        //private void BtnPayDebitClick(object sender, EventArgs e)
        //{
        //    FormMain.ClientInfo.Product = 2;
        //    ChangeView(typeof(FormDebitPayType));
        //}

        private void FormProductsLoad(object sender, EventArgs e)
        {
            HomeButton = false;
            try
            {
                Log.Debug(String.Format("ClientRect: {0}, ClientSize: {1}", ClientRectangle, ClientSize));
                FormMain.ProductUpdate += FormMainOnProductUpdate;

                if (FormMain.Products == null || FormMain.Products.Count == 0)
                {
                    FormMain.ForceCheckProducts();
                }

                if (FormMain.Products != null && FormMain.Products.Count > 0)
                {
                    AddButtons();
                }

                _CheckProductTimer = new Timer(CheckProductTimer, null, 250, 2000);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void CheckProductTimer(object param)
        {
            try
            {
                if (tableLayoutPanel.Controls.Count == 0)
                {
                    FormMain.OpenForm(typeof (FormOutOfOrder));
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void FormMainOnProductUpdate()
        {
            try
            {                                
                AddButtons();
                if (tableLayoutPanel.Controls.Count == 0)
                {
                    FormMain.OpenForm(typeof(FormOutOfOrder));
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }            
        }

        private void AddButtons()
        {
            if (InvokeRequired)
            {
                var form = new AddControlCallBack(AddButtons);
                Invoke(form);
            }
            else
            {
                lock (FormMain.Products)
                {
                    tableLayoutPanel.Controls.Clear();
                    tableLayoutPanel.RowCount = 0;
                    
                    try
                    {
                        using (var font = new Font("Microsoft Sans Serif", 27.75F, FontStyle.Bold))
                        {
                            foreach (var product in FormMain.Products)
                            {
                                string text;

                                if (!String.IsNullOrEmpty(product.NameAz))
                                {
                                    text = product.NameAz;
                                }
                                else if (!String.IsNullOrEmpty(product.NameEn))
                                {
                                    text = product.NameEn;
                                }
                                else
                                {
                                    text = product.NameRu;
                                }
                                Log.Debug(text);

                                var button = new Button
                                    {
                                        Size = new Size(996, 105),
                                        Font = font,
                                        BackColor = Color.Transparent,
                                        Name = product.Id.ToString(CultureInfo.InvariantCulture) + product.Name,
                                        Tag = product,
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
        }

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            try
            {
                var button = (Button)sender;
                var product = (Product)button.Tag;

                FormMain.ClientInfo.Product = product;
                ChangeView(Type.GetType(GetType().Namespace + "." + product.Assembly));
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

        private void FormProductsFormClosed(object sender, FormClosedEventArgs e)
        {
            if (_CheckProductTimer != null)
            {
                _CheckProductTimer.Dispose();
            }
        }
    }
}
