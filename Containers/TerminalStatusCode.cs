using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Admin;

namespace Containers
{
    [Serializable, XmlRoot("TerminalStatusCode")]
    [DataContract(Name = "TerminalStatusCode", Namespace = "urn:CashIn")]
    public class TerminalStatusCode
    {
        private int _Id;
        private int _Type;
        private String _Name;
        private MultiLanguageString _Value;

        [XmlElement(ElementName = "Id")]
        [DataMember(Name = "Id")]
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        [XmlElement(ElementName = "Type")]
        [DataMember(Name = "Type")]
        public int Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        [XmlElement(ElementName = "Name")]
        [DataMember(Name = "Name")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        [XmlElement(ElementName = "Value")]
        [DataMember(Name = "Value")]
        public MultiLanguageString Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        public TerminalStatusCode()
        {
        }

        public TerminalStatusCode(int id, int type, string name, MultiLanguageString value)
        {
            _Id = id;
            _Type = type;
            _Name = name;
            _Value = value;
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, Type: {1}, Name: {2}, Value: {3}", _Id, _Type, _Name, _Value);
        }
    }
}
