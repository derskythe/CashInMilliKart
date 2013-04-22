using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("BonusRequest")]
    [DataContract(Name = "BonusRequest", Namespace = "urn:CashIn")]
    public class BonusRequest : StandardRequest
    {
        private String _CreditNumber;
        private float _Amount;
        private String _Currency;

        public BonusRequest(int terminalId) : base(terminalId)
        {
        }

        [XmlElement(ElementName = "CreditNumber")]
        [DataMember(Name = "CreditNumber")]
        public string CreditNumber
        {
            get { return _CreditNumber; }
            set { _CreditNumber = value; }
        }

        [XmlElement(ElementName = "Amount")]
        [DataMember(Name = "Amount")]
        public float Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }

        [XmlElement(ElementName = "Currency")]
        [DataMember(Name = "Currency")]
        public string Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }

        public BonusRequest()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, CreditNumber: {1}, Amount: {2}, Currency: {3}", base.ToString(), _CreditNumber,
                                 _Amount, _Currency);
        }
    }
}
