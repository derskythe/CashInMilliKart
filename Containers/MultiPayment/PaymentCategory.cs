using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Admin;

namespace Containers.MultiPayment
{
    [Serializable, XmlRoot("PaymentCategory")]
    [DataContract(Name = "PaymentCategory", Namespace = "urn:CashIn")]
    public class PaymentCategory
    {
        private int _Id;
        private String _Name;
        private MultiLanguageString _LocalizedName;
        private List<PaymentService> _Services;

        [XmlElement(ElementName = "Id")]
        [DataMember(Name = "Id")]
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
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

        [XmlArray("Services")]
        [DataMember(Name = "Services")]
        public List<PaymentService> Services
        {
            get { return _Services; }
            set { _Services = value; }
        }

        public PaymentCategory()
        {
        }

        public PaymentCategory(int id, string name, MultiLanguageString localizedName, List<PaymentService> services)
        {
            _Id = id;
            _Name = name;
            _LocalizedName = localizedName;
            _Services = services;
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, LocalizedName: {2}, Services: {3}", _Id, _Name, _LocalizedName,
                                 EnumEx.GetStringFromArray(_Services));
        }
    }
}
