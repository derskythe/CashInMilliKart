using System;

namespace CashInTerminal
{
    public partial class FormDebitCardFull : FormMdiChild
    {
        public FormDebitCardFull()
        {
            InitializeComponent();
        }

        private void BtnDebitClientCodeBackspaceClick(object sender, EventArgs e)
        {
            if (txtDebitClientCode.Text.Length > 0)
            {
                txtDebitClientCode.Text = txtDebitClientCode.Text.Substring(0, txtDebitClientCode.Text.Length - 1);
            }
        }

        private void BtnDebitClientCodeClearClick(object sender, EventArgs e)
        {
            txtDebitClientCode.Text = String.Empty;
        }

        private void BtnDebitClientCode0Click(object sender, EventArgs e)
        {
            txtDebitClientCode.Text += @"0";
        }

        private void BtnDebitClientCode1Click(object sender, EventArgs e)
        {
            txtDebitClientCode.Text += @"1";
        }

        private void BtnDebitClientCode2Click(object sender, EventArgs e)
        {
            txtDebitClientCode.Text += @"2";
        }

        private void BtnDebitClientCode3Click(object sender, EventArgs e)
        {
            txtDebitClientCode.Text += @"3";
        }

        private void BtnDebitClientCode4Click(object sender, EventArgs e)
        {
            txtDebitClientCode.Text += @"4";
        }

        private void BtnDebitClientCode5Click(object sender, EventArgs e)
        {
            txtDebitClientCode.Text += @"5";
        }

        private void BtnDebitClientCode6Click(object sender, EventArgs e)
        {
            txtDebitClientCode.Text += @"6";
        }

        private void BtnDebitClientCode7Click(object sender, EventArgs e)
        {
            txtDebitClientCode.Text += @"7";
        }

        private void BtnDebitClientCode8Click(object sender, EventArgs e)
        {
            txtDebitClientCode.Text += @"8";
        }

        private void BtnDebitClientCode9Click(object sender, EventArgs e)
        {
            txtDebitClientCode.Text += @"9";
        }

        private void BtnDebitClientCodePrevClick(object sender, EventArgs e)
        {
            ChangeView(new FormDebitPayType());
        }

        private void BtnDebitClientCodeNextClick(object sender, EventArgs e)
        {
            if (txtDebitClientCode.Text != @"1234567890123456")
            {
                ChangeView(new FormInvalidNumber());
            }
            else
            {
                FormMain.ClientInfo.AccountNumber = "12345";
                FormMain.ClientInfo.Passport = "12345";
                ChangeView(new FormDebitSelectAccount());
            }
        }

        private void TxtDebitClientCodeTextChanged(object sender, EventArgs e)
        {
            if (txtDebitClientCode.Text.Length == 16)
            {
                btnDebitClientCodeNext.Enabled = true;
            }
            else
            {
                btnDebitClientCodeNext.Enabled = false;
            }
        }        
    }
}
