using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Admin;

namespace Containers.MultiPayment
{
    [Serializable, XmlRoot("PaymentServiceEnum")]
    [DataContract(Name = "PaymentServiceEnum", Namespace = "urn:CashIn")]
    public class PaymentServiceEnum
    {
        private int _FieldId;
        private string _Name;
        private MultiLanguageString _LocalizedName;        
        private string _Value;

        [XmlElement(ElementName = "FieldId")]
        [DataMember(Name = "FieldId")]
        public int FieldId
        {
            get { return _FieldId; }
            set { _FieldId = value; }
        }

        [XmlElement(ElementName = "Name")]
        [DataMember(Name = "Name")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        [XmlElement(ElementName = "LocalizedName")]
        [DataMember(Name = "LocalizedName")]
        public MultiLanguageString LocalizedName
        {
            get { return _LocalizedName; }
            set { _LocalizedName = value; }
        }

        [XmlElement(ElementName = "Value")]
        [DataMember(Name = "Value")]
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        public PaymentServiceEnum()
        {
        }

        public PaymentServiceEnum(int fieldId, string name, MultiLanguageString localizedName, string value)
        {
            _FieldId = fieldId;
            _Name = name;
            _LocalizedName = localizedName;
            _Value = value;
        }

        public override string ToString()
        {
            return string.Format("FieldId: {0}, Name: {1}, LocalizedName: {2}, Value: {3}", _FieldId, _Name,
                                 _LocalizedName, _Value);
        }
    }
}
