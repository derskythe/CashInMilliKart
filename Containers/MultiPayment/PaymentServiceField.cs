using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Admin;

namespace Containers.MultiPayment
{
    [Serializable, XmlRoot("PaymentServiceField")]
    [DataContract(Name = "PaymentServiceField", Namespace = "urn:CashIn")]
    public class PaymentServiceField
    {
        private int _Id;
        private int _ServiceId;
        private MultiLanguageString _LocalizedName;
        private string _Name;
        private string _ServiceName;
        private string _Type;
        private string _Regexp;
        private string _DefaultValue;
        private int _OrderNum;
        private List<PaymentServiceEnum> _FieldValues;
        private string _NormalizeRegexp;
        private string _NormalizePattern;

        [XmlElement(ElementName = "Id")]
        [DataMember(Name = "Id")]
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        [XmlElement(ElementName = "ServiceId")]
        [DataMember(Name = "ServiceId")]
        public int ServiceId
        {
            get { return _ServiceId; }
            set { _ServiceId = value; }
        }

        [XmlElement(ElementName = "LocalizedName")]
        [DataMember(Name = "LocalizedName")]
        public MultiLanguageString LocalizedName
        {
            get { return _LocalizedName; }
            set { _LocalizedName = value; }
        }

        [XmlElement(ElementName = "Name")]
        [DataMember(Name = "Name")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        [XmlElement(ElementName = "ServiceName")]
        [DataMember(Name = "ServiceName")]
        public string ServiceName
        {
            get { return _ServiceName; }
            set { _ServiceName = value; }
        }

        [XmlElement(ElementName = "Type")]
        [DataMember(Name = "Type")]
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        [XmlElement(ElementName = "Regexp")]
        [DataMember(Name = "Regexp")]
        public string Regexp
        {
            get { return _Regexp; }
            set { _Regexp = value; }
        }

        [XmlElement(ElementName = "DefaultValue")]
        [DataMember(Name = "DefaultValue")]
        public string DefaultValue
        {
            get { return _DefaultValue; }
            set { _DefaultValue = value; }
        }

        [XmlElement(ElementName = "OrderNum")]
        [DataMember(Name = "OrderNum")]
        public int OrderNum
        {
            get { return _OrderNum; }
            set { _OrderNum = value; }
        }

        [XmlElement(ElementName = "NormalizeRegexp")]
        [DataMember(Name = "NormalizeRegexp")]
        public string NormalizeRegexp
        {
            get { return _NormalizeRegexp; }
            set { _NormalizeRegexp = value; }
        }

        [XmlElement(ElementName = "NormalizePattern")]
        [DataMember(Name = "NormalizePattern")]
        public string NormalizePattern
        {
            get { return _NormalizePattern; }
            set { _NormalizePattern = value; }
        }

        [XmlArray("FieldValues")]
        [DataMember(Name = "FieldValues")]
        public List<PaymentServiceEnum> FieldValues
        {
            get { return _FieldValues; }
            set { _FieldValues = value; }
        }

        public PaymentServiceField()
        {
        }

        public PaymentServiceField(int id, int serviceId, MultiLanguageString localizedName, string name, string serviceName, string type, string regexp, string defaultValue, int orderNum, List<PaymentServiceEnum> fieldValues, string normalizeRegexp, string normalizePattern)
        {
            _Id = id;
            _ServiceId = serviceId;
            _LocalizedName = localizedName;
            _Name = name;
            _ServiceName = serviceName;
            _Type = type;
            _Regexp = regexp;
            _DefaultValue = defaultValue;
            _OrderNum = orderNum;
            _FieldValues = fieldValues;
            _NormalizeRegexp = normalizeRegexp;
            _NormalizePattern = normalizePattern;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Id: {0}, ServiceId: {1}, LocalizedName: {2}, Name: {3}, ServiceName: {4}, Type: {5}, Regexp: {6}, DefaultValue: {7}, OrderNum: {8}, FieldValues: {9}, NormalizeRegexp: {10}, NormalizePattern: {11}",
                    _Id, _ServiceId, _LocalizedName, _Name, _ServiceName, _Type, _Regexp, _DefaultValue, _OrderNum,
                    EnumEx.GetStringFromArray(_FieldValues), _NormalizeRegexp, _NormalizePattern);
        }
    }
}
