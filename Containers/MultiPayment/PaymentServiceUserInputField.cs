using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers.MultiPayment
{
    [Serializable, XmlRoot("PaymentServiceUserInputField")]
    [DataContract(Name = "PaymentServiceUserInputField", Namespace = "urn:CashIn")]
    public class PaymentServiceUserInputField
    {
        private string _Name;
        private string _Value;

        [XmlElement(ElementName = "Name")]
        [DataMember(Name = "Name")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        [XmlElement(ElementName = "Value")]
        [DataMember(Name = "Value")]
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        public PaymentServiceUserInputField()
        {
        }

        public PaymentServiceUserInputField(string name, string value)
        {
            _Name = name;
            _Value = value;
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, Value: {1}", _Name, _Value);
        }
    }
}
