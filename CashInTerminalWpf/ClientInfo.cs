using System;
using CashInTerminalWpf.CashIn;
using Containers.Enums;

namespace CashInTerminalWpf
{
    public class ClientInfo
    {
        private Product _Product;
        private PaymentCategory _PaymentCategory;
        private PaymentService _PaymentService;
        private String _AccountNumber;
        private String _Passport;
        //private String _CreditAccountNumber;
        private String _CurrentCurrency;
        private int _CashCodeAmount;
        private long _PaymentId;
        private int _OrderNumber;
        private String _TransactionId;
        //private DebitPayType _DebitPayType;
        private PaymentOperationType _PaymentOperationType;
        private CashIn.ClientInfo _Client;

        public Product Product
        {
            get { return _Product; }
            set { _Product = value; }
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

        //public string CreditAccountNumber
        //{
        //    get { return _CreditAccountNumber; }
        //    set { _CreditAccountNumber = value; }
        //}

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

        //public DebitPayType DebitPayType
        //{
        //    get { return _DebitPayType; }
        //    set { _DebitPayType = value; }
        //}

        public PaymentOperationType PaymentOperationType
        {
            get { return _PaymentOperationType; }
            set { _PaymentOperationType = value; }
        }

        public CashIn.ClientInfo Client
        {
            get { return _Client; }
            set { _Client = value; }
        }

        public PaymentCategory PaymentCategory
        {
            get { return _PaymentCategory; }
            set { _PaymentCategory = value; }
        }

        public PaymentService PaymentService
        {
            get { return _PaymentService; }
            set { _PaymentService = value; }
        }

        public ClientInfo()
        {
        }

        public ClientInfo(Product product, string accountNumber, string passport, string currentCurrency, int cashCodeAmount, long paymentId, int orderNumber, string transactionId)
        {
            _Product = product;
            _AccountNumber = accountNumber;
            _Passport = passport;
            //_CreditAccountNumber = creditAccountNumber;
            _CurrentCurrency = currentCurrency;
            _CashCodeAmount = cashCodeAmount;
            _PaymentId = paymentId;
            _OrderNumber = orderNumber;
            _TransactionId = transactionId;
            //_DebitPayType = debitPayType;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Product: {0}, AccountNumber: {1}, Passport: {2}, CurrentCurrency: {3}, CashCodeAmount: {4}, PaymentId: {5}, TransactionId: {6}, PaymentOperationType: {7}, Client: {8}, OrderNumber: {9}",
                    _Product, _AccountNumber, _Passport, _CurrentCurrency, _CashCodeAmount, _PaymentId, _TransactionId,
                    _PaymentOperationType, _Client, _OrderNumber);
        }
    }
}
