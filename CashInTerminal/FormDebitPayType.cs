using System;
using CashInTerminal.Enums;

namespace CashInTerminal
{
    public partial class FormDebitPayType : FormMdiChild
    {
        public FormDebitPayType()
        {
            InitializeComponent();
        }

        private void BtnByCardFullClick(object sender, EventArgs e)
        {
            FormMain.ClientInfo.DebitPayType = DebitPayType.ByCardFull;
            ChangeView(new FormDebitCardFull());
        }

        private void BtnByCardAccountNumberClick(object sender, EventArgs e)
        {
            FormMain.ClientInfo.DebitPayType = DebitPayType.ByCardAccount;
            ChangeView(new FormClientCode());
        }

        private void BtnCurrentClick(object sender, EventArgs e)
        {
            FormMain.ClientInfo.DebitPayType = DebitPayType.Current;
            ChangeView(new FormClientCode());
        }

        private void BtnDepositClick(object sender, EventArgs e)
        {
            FormMain.ClientInfo.DebitPayType = DebitPayType.DebitAccount;
            ChangeView(new FormClientCode());
        }
    }
}
