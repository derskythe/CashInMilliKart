using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("Encashment")]
    [DataContract(Name = "Encashment", Namespace = "urn:CashIn")]
    public class Encashment : StandardRequest
    {
        private int _Id;
        private int _UserId;
        private String _Username;
        private Terminal _Terminal;
        private EncashmentCurrency[] _Currencies;
        private DateTime _InsertDate;
        private List<Banknote> _Banknotes;
        private int _Count;

        [XmlElement(ElementName = "Id")]
        [DataMember(Name = "Id")]
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        [XmlElement(ElementName = "InsertDate")]
        [DataMember(Name = "InsertDate")]
        public DateTime InsertDate
        {
            get { return _InsertDate; }
            set { _InsertDate = value; }
        }

        [XmlElement(ElementName = "UserId")]
        [DataMember(Name = "UserId")]
        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        [XmlArray("Currencies")]
        [DataMember(Name = "Currencies")]
        public EncashmentCurrency[] Currencies
        {
            get { return _Currencies; }
            set { _Currencies = value; }
        }

        [XmlElement(ElementName = "Username")]
        [DataMember(Name = "Username")]
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        [XmlElement(ElementName = "Terminal")]
        [DataMember(Name = "Terminal")]
        public Terminal Terminal
        {
            get { return _Terminal; }
            set { _Terminal = value; }
        }

        [XmlArray("Banknotes")]
        [DataMember(Name = "Banknotes")]
        public List<Banknote> Banknotes
        {
            get { return _Banknotes; }
            set { _Banknotes = value; }
        }

        [XmlElement(ElementName = "Count")]
        [DataMember(Name = "Count")]
        public int Count
        {
            get { return _Count; }
            set { _Count = value; }
        }

        public Encashment()
        {
        }

        public Encashment(int terminalId, int id, int userId, string username, Terminal terminal, EncashmentCurrency[] currencies, DateTime insertDate) : base(terminalId)
        {
            _Id = id;
            _UserId = userId;
            _Username = username;
            _Terminal = terminal;
            _Currencies = currencies;
            _InsertDate = insertDate;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "{0}, Id: {1}, UserId: {2}, Username: {3}, Terminal: {4}, Currencies: {5}, InsertDate: {6}, Banknotes: {7}, Count: {8}",
                    base.ToString(), _Id, _UserId, _Username, _Terminal, EnumEx.GetStringFromArray(_Currencies),
                    _InsertDate, EnumEx.GetStringFromArray(_Banknotes), _Count);
        }
    }
}
