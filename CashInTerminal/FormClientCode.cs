using System;
using System.Windows.Forms;

namespace CashInTerminal
{
    public partial class FormClientCode : FormMdiChild
    {
        private TextBox _SelectedBox;

        public FormClientCode()
        {
            InitializeComponent();
        }

        private TextBox SelectedBox
        {
            get { return _SelectedBox ?? (_SelectedBox = txtClientCodeClient); }
            set { _SelectedBox = value; }
        }

        private void TxtClientCodeClientClick(object sender, EventArgs e)
        {
            SelectedBox = txtClientCodeClient;
        }

        private void TxtClientCodePassportClick(object sender, EventArgs e)
        {
            SelectedBox = txtClientCodePassport;
        }

        private void BtnClientCodeBackspaceClick(object sender, EventArgs e)
        {
            if (SelectedBox.Text.Length > 0)
            {
                SelectedBox.Text = SelectedBox.Text.Substring(0, SelectedBox.Text.Length - 1);
            }
        }

        private void BtnClientCode0Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"0";
        }

        private void BtnClientCodeClearClick(object sender, EventArgs e)
        {
            SelectedBox.Text = String.Empty;
        }

        private void BtnClientCode1Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"1";
        }

        private void BtnClientCode2Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"2";
        }

        private void BtnClientCode3Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"3";
        }

        private void BtnClientCode4Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"4";
        }

        private void BtnClientCode5Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"5";
        }

        private void BtnClientCode6Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"6";
        }

        private void BtnClientCode7Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"7";
        }

        private void BtnClientCode8Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"8";
        }

        private void BtnClientCode9Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"9";
        }

        private void BtnClientCodeBackClick(object sender, EventArgs e)
        {
            switch (FormMain.ClientInfo.ProductCode)
            {
                case 1:
                    ChangeView(typeof(FormProducts));
                    break;

                case 2:
                    ChangeView(typeof(FormDebitPayType));
                    break;
            }
        }

        private void BtnClientCodeNextClick(object sender, EventArgs e)
        {
            if (txtClientCodePassport.Text == @"12345" || txtClientCodeClient.Text == @"12345")
            {
                FormMain.ClientInfo.Passport = txtClientCodePassport.Text;
                FormMain.ClientInfo.AccountNumber = txtClientCodeClient.Text;

                switch (FormMain.ClientInfo.ProductCode)
                {
                    case 1:
                        ChangeView(typeof(FormCreditSelectAccount));

                        break;

                    case 2:
                        ChangeView(typeof(FormDebitSelectAccount));

                        break;
                }
            }
            else
            {
                ChangeView(typeof(FormInvalidNumber));
            }
        }
    }
}
