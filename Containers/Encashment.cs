using System;
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
        private EncashmentCurrency[] _Currencies;
        private DateTime _InsertDate;

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

        public Encashment()
        {
        }

        public Encashment(int terminalId, int userId, EncashmentCurrency[] currencies)
        {
            _TerminalId = terminalId;
            _UserId = userId;
            _Currencies = currencies;
            
        }

        public override string ToString()
        {
            return string.Format("{0}, Id: {1}, UserId: {2}, Currencies: {3}, InsertDate: {4}", base.ToString(), _Id,
                                 _UserId, EnumEx.ToStringArray(_Currencies), _InsertDate);
        }
    }
}
