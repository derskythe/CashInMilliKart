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
                    ChangeView(new FormClientCode());
                    break;

                case 2:
                    if (FormMain.ClientInfo.DebitPayType != DebitPayType.ByCardFull)
                    {
                        ChangeView(new FormClientCode());
                    }
                    else
                    {
                        ChangeView(new FormDebitCardFull());
                    }
                    break;
            }
        }
    }
}
