using System;
using CashInTerminal.CashIn;
using CashInTerminal.Enums;

namespace CashInTerminal
{
    public partial class FormInvalidNumber : FormMdiChild
    {
        public FormInvalidNumber()
        {
            InitializeComponent();
        }

        private void BtbBackClick(object sender, EventArgs e)
        {
            switch (FormMain.ClientInfo.ProductCode)
            {
                case 1:
                    switch (FormMain.ClientInfo.GetClientInfoType)
                    {
                        case GetClientInfoType.ByClientCode:
                            ChangeView(typeof(FormCreditByClientCode));
                            break;

                        case GetClientInfoType.ByPasportAndCreditNumber:
                            ChangeView(typeof(FormCreditByPassport1));
                            break;

                        case GetClientInfoType.Bolcard:
                            ChangeView(typeof(FormCreditByBolcard));
                            break;

                        default:
                            ChangeView(typeof(FormCreditTypeSelect));
                            break;
                    }
                    break;

                case 2:
                    if (FormMain.ClientInfo.DebitPayType != DebitPayType.ByCardFull)
                    {
                        ChangeView(typeof(FormClientCode));
                    }
                    else
                    {
                        ChangeView(typeof(FormDebitCardFull));
                    }
                    break;
            }
        }
    }
}
