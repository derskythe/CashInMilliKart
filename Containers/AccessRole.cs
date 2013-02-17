using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("AccessRole")]
    [DataContract(Name = "AccessRole", Namespace = "urn:CashIn")]
    public class AccessRole
    {
        private int _Id;
        private String _Section;
        private String _Name;
        private String _NameAz;
        private String _NameRu;
        private String _NameEn;

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

        [XmlElement(ElementName = "NameAz")]
        [DataMember(Name = "NameAz")]
        public string NameAz
        {
            get { return _NameAz; }
            set { _NameAz = value; }
        }

        [XmlElement(ElementName = "NameRu")]
        [DataMember(Name = "NameRu")]
        public string NameRu
        {
            get { return _NameRu; }
            set { _NameRu = value; }
        }

        [XmlElement(ElementName = "NameEn")]
        [DataMember(Name = "NameEn")]
        public string NameEn
        {
            get { return _NameEn; }
            set { _NameEn = value; }
        }

        [XmlElement(ElementName = "Section")]
        [DataMember(Name = "Section")]
        public string Section
        {
            get { return _Section; }
            set { _Section = value; }
        }

        public AccessRole()
        {
        }

        public AccessRole(int id, string section, string name, string nameAz, string nameRu, string nameEn)
        {
            _Id = id;
            _Section = section;
            _Name = name;
            _NameAz = nameAz;
            _NameRu = nameRu;
            _NameEn = nameEn;
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, Section: {1}, Name: {2}, NameAz: {3}, NameRu: {4}, NameEn: {5}", _Id,
                                 _Section, _Name, _NameAz, _NameRu, _NameEn);
        }
    }
}
