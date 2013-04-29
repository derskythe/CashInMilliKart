using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.MultiPayment
{
    [Serializable, XmlRoot("PaymentServiceInfoResponse")]
    [DataContract(Name = "PaymentServiceInfoResponse", Namespace = "urn:CashIn")]
    public class PaymentServiceInfoResponse : StandardResult
    {
        private String _Person;
        private float _Debt;

        public PaymentServiceInfoResponse(ResultCodes code) : base(code)
        {
        }

        [XmlElement(ElementName = "Person")]
        [DataMember(Name = "Person")]
        public string Person
        {
            get { return _Person; }
            set { _Person = value; }
        }

        [XmlElement(ElementName = "Debt")]
        [DataMember(Name = "Debt")]
        public float Debt
        {
            get { return _Debt; }
            set { _Debt = value; }
        }

        public PaymentServiceInfoResponse()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Person: {1}, Debt: {2}", base.ToString(), _Person, _Debt);
        }
    }
}
