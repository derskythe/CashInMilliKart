using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("Banknote")]
    [DataContract(Name = "Banknote", Namespace = "urn:CashIn")]
    public class Banknote
    {
        private long _Id;
        private double _Amount;
        private long _TerminalId;
        private DateTime _InsertDate;
        private string _CurrencyId;
        private long _EncashmentId;
        private long _HistoryId;

        [XmlElement(ElementName = "Id")]
        [DataMember(Name = "Id")]
        public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        [XmlElement(ElementName = "Amount")]
        [DataMember(Name = "Amount")]
        public double Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }

        [XmlElement(ElementName = "TerminalId")]
        [DataMember(Name = "TerminalId")]
        public long TerminalId
        {
            get { return _TerminalId; }
            set { _TerminalId = value; }
        }

        [XmlElement(ElementName = "InsertDate")]
        [DataMember(Name = "InsertDate")]
        public DateTime InsertDate
        {
            get { return _InsertDate; }
            set { _InsertDate = value; }
        }

        [XmlElement(ElementName = "CurrencyId")]
        [DataMember(Name = "CurrencyId")]
        public string CurrencyId
        {
            get { return _CurrencyId; }
            set { _CurrencyId = value; }
        }

        [XmlElement(ElementName = "EncashmentId")]
        [DataMember(Name = "EncashmentId")]
        public long EncashmentId
        {
            get { return _EncashmentId; }
            set { _EncashmentId = value; }
        }

        [XmlElement(ElementName = "HistoryId")]
        [DataMember(Name = "HistoryId")]
        public long HistoryId
        {
            get { return _HistoryId; }
            set { _HistoryId = value; }
        }

        public Banknote()
        {
        }

        public Banknote(long id, double amount, long terminalId, DateTime insertDate, string currencyId, long encashmentId, long historyId)
        {
            _Id = id;
            _Amount = amount;
            _TerminalId = terminalId;
            _InsertDate = insertDate;
            _CurrencyId = currencyId;
            _EncashmentId = encashmentId;
            _HistoryId = historyId;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Id: {0}, Amount: {1}, TerminalId: {2}, InsertDate: {3}, CurrencyId: {4}, EncashmentId: {5}, HistoryId: {6}",
                    _Id, _Amount, _TerminalId, _InsertDate, _CurrencyId, _EncashmentId, _HistoryId);
        }
    }
}
