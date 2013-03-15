using System;
using CashInTerminal.CashIn;

namespace CashInTerminal
{
    public partial class FormCreditTypeSelect : FormMdiChild
    {
        public FormCreditTypeSelect()
        {
            InitializeComponent();
        }        

        private void BtnClientNumberClick(object sender, EventArgs e)
        {
            FormMain.ClientInfo.GetClientInfoType = GetClientInfoType.ByClientCode;
            ChangeView(typeof(FormCreditByClientCode));
        }

        private void BtnCreditNumberAndPasportClick(object sender, EventArgs e)
        {
            FormMain.ClientInfo.GetClientInfoType = GetClientInfoType.ByPasportAndCreditNumber;
            ChangeView(typeof(FormCreditByPassport1));
        }

        private void BtnBolCardClick(object sender, EventArgs e)
        {
            FormMain.ClientInfo.GetClientInfoType = GetClientInfoType.Bolcard;
            ChangeView(typeof(FormCreditByBolcard));
        }

        private void BtnBackClick(object sender, EventArgs e)
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
    }
}
