using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("PaymentInfoByProducts")]
    [DataContract(Name = "PaymentInfoByProducts", Namespace = "urn:CashIn")]
    public class PaymentInfoByProducts : StandardRequest
    {
        private string _TransactionId;
        private int _ProductId;
        private String _Currency;
        private float _CurrencyRate;
        private int _Amount;
        private DateTime _TerminalDate;
        private String[] _Values;
        private int[] _Banknotes;

        [XmlElement(ElementName = "TransactionId")]
        [DataMember(Name = "TransactionId")]
        public string TransactionId
        {
            get { return _TransactionId; }
            set { _TransactionId = value; }
        }

        [XmlElement(ElementName = "ProductId")]
        [DataMember(Name = "ProductId")]
        public int ProductId
        {
            get { return _ProductId; }
            set { _ProductId = value; }
        }

        [XmlElement(ElementName = "Currency")]
        [DataMember(Name = "Currency")]
        public string Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }

        [XmlElement(ElementName = "CurrencyRate")]
        [DataMember(Name = "CurrencyRate")]
        public float CurrencyRate
        {
            get { return _CurrencyRate; }
            set { _CurrencyRate = value; }
        }

        [XmlElement(ElementName = "Amount")]
        [DataMember(Name = "Amount")]
        public int Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }

        [XmlElement(ElementName = "TerminalDate")]
        [DataMember(Name = "TerminalDate")]
        public DateTime TerminalDate
        {
            get { return _TerminalDate; }
            set { _TerminalDate = value; }
        }

        [XmlArray("Values")]
        [DataMember(Name = "Values")]
        public string[] Values
        {
            get { return _Values; }
            set { _Values = value; }
        }

        [XmlArray("Banknotes")]
        [DataMember(Name = "Banknotes")]
        public int[] Banknotes
        {
            get { return _Banknotes; }
            set { _Banknotes = value; }
        }

        public PaymentInfoByProducts()
        {
        }

        public PaymentInfoByProducts(string transactionId, int terminalId, int productId, string currency, float currencyRate, int amount, DateTime terminalDate, string[] values, int[] banknotes)
        {
            _TransactionId = transactionId;
            _TerminalId = terminalId;
            _ProductId = productId;
            _Currency = currency;
            _CurrencyRate = currencyRate;
            _Amount = amount;
            _TerminalDate = terminalDate;
            _Values = values;
            _Banknotes = banknotes;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "{0}, TransactionId: {1}, ProductId: {2}, Currency: {3}, CurrencyRate: {4}, Amount: {5}, TerminalDate: {6}, Values: {7}, Banknotes: {8}",
                    base.ToString(), _TransactionId, _ProductId, _Currency, _CurrencyRate, _Amount, _TerminalDate,
                    EnumEx.ToStringArray(_Values), EnumEx.ToStringArray(_Banknotes));
        }
    }
}
