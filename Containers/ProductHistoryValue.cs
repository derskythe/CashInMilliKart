using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("ProductHistoryValue")]
    [DataContract(Name = "ProductHistoryValue", Namespace = "urn:CashIn")]
    public class ProductHistoryValue
    {
        private decimal _Id;
        private string _Value;

        [XmlElement(ElementName = "Id")]
        [DataMember(Name = "Id")]
        public decimal Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        [XmlElement(ElementName = "Value")]
        [DataMember(Name = "Value")]
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        public ProductHistoryValue()
        {
        }

        public ProductHistoryValue(decimal id, string value)
        {
            _Id = id;
            _Value = value;
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, Value: {1}", _Id, _Value);
        }
    }
}
