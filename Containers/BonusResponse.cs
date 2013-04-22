using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers
{
    [Serializable, XmlRoot("BonusResponse")]
    [DataContract(Name = "BonusResponse", Namespace = "urn:CashIn")]
    public class BonusResponse : StandardResult
    {
        private float _Bonus;

        public BonusResponse(ResultCodes code) : base(code)
        {
        }

        [XmlElement(ElementName = "Bonus")]
        [DataMember(Name = "Bonus")]
        public float Bonus
        {
            get { return _Bonus; }
            set { _Bonus = value; }
        }

        public BonusResponse()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Bonus: {1}", base.ToString(), _Bonus);
        }
    }
}
