using System;
using System.Windows.Forms;

namespace CashInTerminal.BaseForms
{
    public partial class FormEnterClientCode : FormMdiChild
    {
        private TextBox _SelectedBox;

        private TextBox SelectedBox
        {
            get { return _SelectedBox ?? (_SelectedBox = txtClientCodeClient); }
            set { _SelectedBox = value; }
        }

        public FormEnterClientCode()
        {
            InitializeComponent();
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

        protected virtual void BtnClientCodeBackClick(object sender, EventArgs e)
        {            
        }

        protected virtual void BtnClientCodeNextClick(object sender, EventArgs e)
        {            
        }

        protected void TxtClientCodeClientClick(object sender, EventArgs e)
        {
            SelectedBox = txtClientCodeClient;
        }

        private void FormEnterClientCodeLoad(object sender, EventArgs e)
        {

        }
    }
}
