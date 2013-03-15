using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("ClientInfo")]
    [DataContract(Name = "ClientInfo", Namespace = "urn:CashIn")]
    public class ClientInfo
    {
        private string _CreditNumber;
        private string _FullName;
        private string _PassportNumber;
        private string _CreditAccount;
        private string _ClientAccount;
        private double _AmountLeft;
        private double _AmountLate;
        private string _Currency;
        private DateTime _BeginDate;
        private double _CurrencyRate;
        private string _ClientCode;
        private double _CreditAmount;
        private string _CreditName;

        [XmlElement(ElementName = "CreditNumber")]
        [DataMember(Name = "CreditNumber")]
        public string CreditNumber
        {
            get { return _CreditNumber; }
            set { _CreditNumber = value; }
        }

        [XmlElement(ElementName = "FullName")]
        [DataMember(Name = "FullName")]
        public string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }

        [XmlElement(ElementName = "PassportNumber")]
        [DataMember(Name = "PassportNumber")]
        public string PassportNumber
        {
            get { return _PassportNumber; }
            set { _PassportNumber = value; }
        }

        [XmlElement(ElementName = "CreditAccount")]
        [DataMember(Name = "CreditAccount")]
        public string CreditAccount
        {
            get { return _CreditAccount; }
            set { _CreditAccount = value; }
        }

        [XmlElement(ElementName = "ClientAccount")]
        [DataMember(Name = "ClientAccount")]
        public string ClientAccount
        {
            get { return _ClientAccount; }
            set { _ClientAccount = value; }
        }

        [XmlElement(ElementName = "AmountLeft")]
        [DataMember(Name = "AmountLeft")]
        public double AmountLeft
        {
            get { return _AmountLeft; }
            set { _AmountLeft = value; }
        }

        [XmlElement(ElementName = "AmountLate")]
        [DataMember(Name = "AmountLate")]
        public double AmountLate
        {
            get { return _AmountLate; }
            set { _AmountLate = value; }
        }

        [XmlElement(ElementName = "Currency")]
        [DataMember(Name = "Currency")]
        public string Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }

        [XmlElement(ElementName = "BeginDate")]
        [DataMember(Name = "BeginDate")]
        public DateTime BeginDate
        {
            get { return _BeginDate; }
            set { _BeginDate = value; }
        }

        [XmlElement(ElementName = "CurrencyRate")]
        [DataMember(Name = "CurrencyRate")]
        public double CurrencyRate
        {
            get { return _CurrencyRate; }
            set { _CurrencyRate = value; }
        }

        [XmlElement(ElementName = "ClientCode")]
        [DataMember(Name = "ClientCode")]
        public string ClientCode
        {
            get { return _ClientCode; }
            set { _ClientCode = value; }
        }

        [XmlElement(ElementName = "CreditAmount")]
        [DataMember(Name = "CreditAmount")]
        public double CreditAmount
        {
            get { return _CreditAmount; }
            set { _CreditAmount = value; }
        }

        [XmlElement(ElementName = "CreditName")]
        [DataMember(Name = "CreditName")]
        public string CreditName
        {
            get { return _CreditName; }
            set { _CreditName = value; }
        }

        public ClientInfo()
        {
        }

        public ClientInfo(string creditNumber, string fullName, string passportNumber, string creditAccount, string clientAccount, double amountLeft, double amountLate, string currency, DateTime beginDate, double currencyRate, string clientCode, double creditAmount, string creditName)
        {
            _CreditNumber = creditNumber;
            _FullName = fullName;
            _PassportNumber = passportNumber;
            _CreditAccount = creditAccount;
            _ClientAccount = clientAccount;
            _AmountLeft = amountLeft;
            _AmountLate = amountLate;
            _Currency = currency;
            _BeginDate = beginDate;
            _CurrencyRate = currencyRate;
            _ClientCode = clientCode;
            _CreditAmount = creditAmount;
            _CreditName = creditName;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "CreditNumber: {0}, FullName: {1}, PassportNumber: {2}, CreditAccount: {3}, ClientAccount: {4}, AmountLeft: {5}, AmountLate: {6}, Currency: {7}, BeginDate: {8}, CurrencyRate: {9}, ClientCode: {10}, CreditAmount: {11}, CreditName: {12}",
                    _CreditNumber, _FullName, _PassportNumber, _CreditAccount, _ClientAccount, _AmountLeft, _AmountLate,
                    _Currency, _BeginDate, _CurrencyRate, _ClientCode, _CreditAmount, _CreditName);
        }
    }
}
