using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("BanknoteSummary")]
    [DataContract(Name = "BanknoteSummary", Namespace = "urn:CashIn")]
    public class BanknoteSummary
    {
        private float _Amount;
        private int _EncashmentId;
        private int _HistoryId;
        private int _TerminalId;
        private String _Currency;
        private int _CountAll;

        [XmlElement(ElementName = "Amount")]
        [DataMember(Name = "Amount")]
        public float Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }

        [XmlElement(ElementName = "EncashmentId")]
        [DataMember(Name = "EncashmentId")]
        public int EncashmentId
        {
            get { return _EncashmentId; }
            set { _EncashmentId = value; }
        }

        [XmlElement(ElementName = "HistoryId")]
        [DataMember(Name = "HistoryId")]
        public int HistoryId
        {
            get { return _HistoryId; }
            set { _HistoryId = value; }
        }

        [XmlElement(ElementName = "TerminalId")]
        [DataMember(Name = "TerminalId")]
        public int TerminalId
        {
            get { return _TerminalId; }
            set { _TerminalId = value; }
        }

        [XmlElement(ElementName = "Currency")]
        [DataMember(Name = "Currency")]
        public string Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }

        [XmlElement(ElementName = "CountAll")]
        [DataMember(Name = "CountAll")]
        public int CountAll
        {
            get { return _CountAll; }
            set { _CountAll = value; }
        }

        public BanknoteSummary()
        {
        }

        public BanknoteSummary(float amount, int encashmentId, int historyId, int terminalId, string currency, int countAll)
        {
            _Amount = amount;
            _EncashmentId = encashmentId;
            _HistoryId = historyId;
            _TerminalId = terminalId;
            _Currency = currency;
            _CountAll = countAll;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Amount: {0}, EncashmentId: {1}, HistoryId: {2}, TerminalId: {3}, Currency: {4}, CountAll: {5}",
                    _Amount, _EncashmentId, _HistoryId, _TerminalId, _Currency, _CountAll);
        }
    }
}
