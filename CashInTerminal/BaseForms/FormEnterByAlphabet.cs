using System;
using System.Windows.Forms;

namespace CashInTerminal.BaseForms
{
    public partial class FormEnterByAlphabet : FormMdiChild
    {
        private TextBox _SelectedBox;

        private TextBox SelectedBox
        {
            get { return _SelectedBox ?? (_SelectedBox = txtClientCodeClient); }
            set { _SelectedBox = value; }
        }

        public FormEnterByAlphabet()
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
            AddText(sender);
        }

        private void BtnClientCodeClearClick(object sender, EventArgs e)
        {
            SelectedBox.Text = String.Empty;
        }

        private void BtnClientCode1Click(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnClientCode2Click(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnClientCode3Click(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnClientCode4Click(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnClientCode5Click(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnClientCode6Click(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnClientCode7Click(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnClientCode8Click(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnClientCode9Click(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnAClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnBClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnCClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnDClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnEClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnFClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnGClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnHClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnIClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnJClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnKClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnLClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnMClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnNClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnOClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnPClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnQClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnRClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnSClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnTClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnUClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnVClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnWClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnXClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnYClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnZClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void BtnPlusClick(object sender, EventArgs e)
        {
            AddText(sender);
        }

        private void AddText(object sender)
        {
            if (sender == null)
            {
                Log.Error("Sender is null");
                return;
            }
            var t = (TextBox)sender;
            SelectedBox.Text += t.Text;
        }

        #endregion
       
        protected virtual void BtnClientCodeBackClick(object sender, EventArgs e)
        {
        }

        protected virtual void BtnClientCodeNextClick(object sender, EventArgs e)
        {            
        }

        private void TxtClientCodeClientClick(object sender, EventArgs e)
        {
            SelectedBox = txtClientCodeClient;
        }        
    }
}
