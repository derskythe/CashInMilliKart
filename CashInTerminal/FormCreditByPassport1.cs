using System;
using System.Windows.Forms;

namespace CashInTerminal
{
    public partial class FormCreditByPassport1 : FormMdiChild
    {
        private TextBox _SelectedBox;

        private TextBox SelectedBox
        {
            get { return _SelectedBox ?? (_SelectedBox = txtClientCodeClient); }
            set { _SelectedBox = value; }
        }

        public FormCreditByPassport1()
        {
            InitializeComponent();
        }

        #region Input

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

        private void BtnAClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"A";
        }

        private void BtnBClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"B";
        }

        private void BtnCClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"C";
        }

        private void BtnDClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"D";
        }

        private void BtnEClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"E";
        }

        private void BtnFClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"F";
        }

        private void BtnGClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"G";
        }

        private void BtnHClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"H";
        }

        private void BtnIClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"I";
        }

        private void BtnJClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"J";
        }

        private void BtnKClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"K";
        }

        private void BtnLClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"L";
        }

        private void BtnMClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"M";
        }

        private void BtnNClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"N";
        }

        private void BtnOClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"O";
        }

        private void BtnPClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"P";
        }

        private void BtnQClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"Q";
        }

        private void BtnRClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"R";
        }

        private void BtnSClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"S";
        }

        private void BtnTClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"T";
        }

        private void BtnUClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"U";
        }

        private void BtnVClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"V";
        }

        private void BtnWClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"W";
        }

        private void BtnXClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"X";
        }

        private void BtnYClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"Y";
        }

        private void BtnZClick(object sender, EventArgs e)
        {
            SelectedBox.Text += @"Z";
        }

        #endregion

        private void FormCreditByPassport1Load(object sender, EventArgs e)
        {

        }

        private void BtnClientCodeBackClick(object sender, EventArgs e)
        {
            ChangeView(typeof(FormCreditTypeSelect));
        }

        private void BtnClientCodeNextClick(object sender, EventArgs e)
        {
            if (SelectedBox.Text.Length > 4)
            {
                FormMain.ClientInfo.AccountNumber = SelectedBox.Text;
                ChangeView(typeof(FormCreditByPassport2));
            }
        }

        private void TxtClientCodeClientClick(object sender, EventArgs e)
        {
            SelectedBox = txtClientCodeClient;
        }
    }
}
