using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using CashInTerminal.Enums;

namespace CashInTerminal
{
    public partial class FormCurrencySelect : BaseForms.FormMdiChild
    {
        public FormCurrencySelect()
        {
            InitializeComponent();
        }

        private void FormCurrencySelectLoad(object sender, EventArgs e)
        {
            try
            {
                if (FormMain.Currencies != null && FormMain.Currencies.Count > 0)
                {
                    AddButtons();
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void AddButtons()
        {
            lock (FormMain.Currencies)
            {
                tableLayoutPanel.Controls.Clear();
                tableLayoutPanel.RowCount = 0;

                try
                {
                    using (var font = new Font("Microsoft Sans Serif", 27.75F, FontStyle.Bold))
                    {
                        foreach (var tag in FormMain.Currencies)
                        {
                            string text = tag.Id;
                            Log.Debug(text);

                            var button = new Button
                            {
                                Size = new Size(996, 105),
                                Font = font,
                                BackColor = Color.Transparent,
                                Name = tag.Id.ToString(CultureInfo.InvariantCulture) + tag.Name,
                                Tag = tag.Id,
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
                FormMain.ClientInfo.CurrentCurrency = (String)button.Tag;
                ChangeView(typeof(FormMoneyInput));
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

        private void BtnBackClick(object sender, EventArgs e)
        {
            switch ((CheckTemplateTypes)FormMain.ClientInfo.Product.CheckType)
            {
                case CheckTemplateTypes.CreditPayment:
                    ChangeView(typeof(FormCreditClientInfo));
                    break;

                case CheckTemplateTypes.DebitPayment:
                    ChangeView(typeof(FormDebitClientInfo));
                    break;
            }
        }
    }
}
