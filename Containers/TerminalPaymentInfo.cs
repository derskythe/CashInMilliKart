using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("TerminalPaymentInfo")]
    [DataContract(Name = "TerminalPaymentInfo", Namespace = "urn:CashIn")]
    public class TerminalPaymentInfo : StandardRequest
    {
        private string _TransactionId;
        private int _ProductId;
        private String _Currency;
        private float _CurrencyRate;
        private int _Amount;
        private int _OperationType;
        private DateTime _TerminalDate;
        private String[] _Values;
        private int[] _Banknotes;
        private String _CreditNumber;
        private int _PaymentServiceId;

        public TerminalPaymentInfo(int terminalId) : base(terminalId)
        {
        }

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

        [XmlElement(ElementName = "OperationType")]
        [DataMember(Name = "OperationType")]
        public int OperationType
        {
            get { return _OperationType; }
            set { _OperationType = value; }
        }

        [XmlElement(ElementName = "TerminalDate")]
        [DataMember(Name = "TerminalDate")]
        public DateTime TerminalDate
        {
            get { return _TerminalDate; }
            set { _TerminalDate = value; }
        }

        [XmlArray(ElementName = "Values")]
        [DataMember(Name = "Values")]
        public string[] Values
        {
            get { return _Values; }
            set { _Values = value; }
        }

        [XmlArray(ElementName = "Banknotes")]
        [DataMember(Name = "Banknotes")]
        public int[] Banknotes
        {
            get { return _Banknotes; }
            set { _Banknotes = value; }
        }

        [XmlElement(ElementName = "CreditNumber")]
        [DataMember(Name = "CreditNumber")]
        public string CreditNumber
        {
            get { return _CreditNumber; }
            set { _CreditNumber = value; }
        }

        [XmlElement(ElementName = "PaymentServiceId")]
        [DataMember(Name = "PaymentServiceId")]
        public int PaymentServiceId
        {
            get { return _PaymentServiceId; }
            set { _PaymentServiceId = value; }
        }

        public TerminalPaymentInfo()
        {
        }

        public override string ToString()
        {
            return
                string.Format(
                    "{0}, TransactionId: {1}, ProductId: {2}, Currency: {3}, CurrencyRate: {4}, Amount: {5}, OperationType: {6}, TerminalDate: {7}, Values: {8}, Banknotes: {9}, CreditNumber: {10}, PaymentServiceId: {11}",
                    base.ToString(), _TransactionId, _ProductId, _Currency, _CurrencyRate, _Amount, _OperationType,
                    _TerminalDate, EnumEx.GetStringFromArray(_Values), EnumEx.GetStringFromArray(_Banknotes),
                    _CreditNumber, _PaymentServiceId);
        }
    }
}
