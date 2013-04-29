using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Admin;

namespace Containers.MultiPayment
{
    [Serializable, XmlRoot("PaymentFixedAmount")]
    [DataContract(Name = "PaymentFixedAmount", Namespace = "urn:CashIn")]
    public class PaymentFixedAmount
    {
        private int _ServiceId;
        private int _Amount;
        private MultiLanguageString _Name;

        [XmlElement(ElementName = "ServiceId")]
        [DataMember(Name = "ServiceId")]
        public int ServiceId
        {
            get { return _ServiceId; }
            set { _ServiceId = value; }
        }

        [XmlElement(ElementName = "Amount")]
        [DataMember(Name = "Amount")]
        public int Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }

        [XmlElement(ElementName = "Name")]
        [DataMember(Name = "Name")]
        public MultiLanguageString Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public PaymentFixedAmount()
        {
        }

        public PaymentFixedAmount(int serviceId, int amount, MultiLanguageString name)
        {
            _ServiceId = serviceId;
            _Amount = amount;
            _Name = name;
        }

        public override string ToString()
        {
            return string.Format("ServiceId: {0}, Amount: {1}, Name: {2}", _ServiceId, _Amount, _Name);
        }
    }
}
