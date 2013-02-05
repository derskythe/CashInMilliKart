using System;

namespace CashInTerminal
{
    public class ClientInfo
    {
        private int _ProductCode;
        private String _AccountNumber;
        private String _Passport;
        private String _CreditAccountNumber;
        private String _CurrentCurrency;

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

        public ClientInfo()
        {
        }

        public ClientInfo(int productCode, string accountNumber, string passport)
        {
            _ProductCode = productCode;
            _AccountNumber = accountNumber;
            _Passport = passport;
        }

        public override string ToString()
        {
            return string.Format("ProductCode: {0}, AccountNumber: {1}, Passport: {2}, CreditAccountNumber: {3}",
                                 _ProductCode, _AccountNumber, _Passport, _CreditAccountNumber);
        }
    }
}
