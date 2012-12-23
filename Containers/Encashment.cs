using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("Encashment")]
    [DataContract(Name = "Encashment", Namespace = "urn:CashIn")]
    public class Encashment : StandardRequest
    {
        private int _UserId;
        private string[] _Currencies;
        private int[] _Amounts;

        [XmlElement(ElementName = "UserId")]
        [DataMember(Name = "UserId")]
        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        [XmlArray("Currencies")]
        [DataMember(Name = "Currencies")]
        public string[] Currencies
        {
            get { return _Currencies; }
            set { _Currencies = value; }
        }

        [XmlArray("Amounts")]
        [DataMember(Name = "Amounts")]
        public int[] Amounts
        {
            get { return _Amounts; }
            set { _Amounts = value; }
        }

        public Encashment()
        {
        }

        public Encashment(int terminalId, int userId, string[] currencies, int[] amounts)
        {
            _TerminalId = terminalId;
            _UserId = userId;
            _Currencies = currencies;
            _Amounts = amounts;
        }

        public override string ToString()
        {
            return string.Format("TerminalId: {0}, UserId: {1}, Currencies: {2}, Amounts: {3}", _TerminalId, _UserId,
                                 EnumEx.ToStringArray(_Currencies), EnumEx.ToStringArray(_Amounts));
        }
    }
}
