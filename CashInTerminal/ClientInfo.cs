using System;
using CashInTerminal.Enums;

namespace CashInTerminal
{
    public class ClientInfo
    {
        private int _ProductCode;
        private String _AccountNumber;
        private String _Passport;
        private String _CreditAccountNumber;
        private String _CurrentCurrency;
        private int _CashCodeAmount;
        private long _PaymentId;
        private int _OrderNumber;
        private String _TransactionId;
        private DebitPayType _DebitPayType;

        public int ProductCode
        {
            get { return _ProductCode; }
            set { _ProductCode = value; }
        }

        public string AccountNumber
        {
            get { return _AccountNumber; }
            set { _AccountNumber = value; }
        }

        public string Passport
        {
            get { return _Passport; }
            set { _Passport = value; }
        }

        public string CreditAccountNumber
        {
            get { return _CreditAccountNumber; }
            set { _CreditAccountNumber = value; }
        }

        public string CurrentCurrency
        {
            get { return _CurrentCurrency; }
            set { _CurrentCurrency = value; }
        }

        public int CashCodeAmount
        {
            get { return _CashCodeAmount; }
            set { _CashCodeAmount = value; }
        }

        public long PaymentId
        {
            get { return _PaymentId; }
            set { _PaymentId = value; }
        }

        public int OrderNumber
        {
            get { return _OrderNumber; }
            set { _OrderNumber = value; }
        }

        public string TransactionId
        {
            get { return _TransactionId; }
            set { _TransactionId = value; }
        }

        public DebitPayType DebitPayType
        {
            get { return _DebitPayType; }
            set { _DebitPayType = value; }
        }

        public ClientInfo()
        {
        }

        public ClientInfo(int productCode, string accountNumber, string passport, string creditAccountNumber, string currentCurrency, int cashCodeAmount, long paymentId, int orderNumber, string transactionId, DebitPayType debitPayType)
        {
            _ProductCode = productCode;
            _AccountNumber = accountNumber;
            _Passport = passport;
            _CreditAccountNumber = creditAccountNumber;
            _CurrentCurrency = currentCurrency;
            _CashCodeAmount = cashCodeAmount;
            _PaymentId = paymentId;
            _OrderNumber = orderNumber;
            _TransactionId = transactionId;
            _DebitPayType = debitPayType;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "ProductCode: {0}, AccountNumber: {1}, Passport: {2}, CreditAccountNumber: {3}, CurrentCurrency: {4}, CashCodeAmount: {5}, PaymentId: {6}, OrderNumber: {7}, TransactionId: {8}, DebitPayType: {9}",
                    _ProductCode, _AccountNumber, _Passport, _CreditAccountNumber, _CurrentCurrency, _CashCodeAmount,
                    _PaymentId, _OrderNumber, _TransactionId, _DebitPayType);
        }
    }
}
