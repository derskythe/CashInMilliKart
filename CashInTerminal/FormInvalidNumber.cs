using System;
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
                    ChangeView(typeof(FormClientCode));
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
