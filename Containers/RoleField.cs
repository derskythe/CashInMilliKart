using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("RoleField")]
    [DataContract(Name = "RoleField", Namespace = "urn:CashIn")]
    public class RoleField
    {
        private String _Section;

        [XmlElement(ElementName = "Section")]
        [DataMember(Name = "Section")]
        public string Section
        {
            get { return _Section; }
            set { _Section = value; }
        }

        public RoleField()
        {
        }

        public RoleField(string section)
        {
            _Section = section;
        }

        public override string ToString()
        {
            return string.Format("Section: {0}", _Section);
        }
    }
}
