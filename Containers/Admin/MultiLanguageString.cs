using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers.Admin
{
    [Serializable, XmlRoot("MultiLanguageString")]
    [DataContract(Name = "MultiLanguageString", Namespace = "urn:CashIn")]
    public class MultiLanguageString
    {
        private string _ValueEn;
        private string _ValueRu;
        private string _ValueAz;

        [XmlElement(ElementName = "ValueEn")]
        [DataMember(Name = "ValueEn")]
        public string ValueEn
        {
            get { return _ValueEn; }
            set { _ValueEn = value; }
        }

        [XmlElement(ElementName = "ValueRu")]
        [DataMember(Name = "ValueRu")]
        public string ValueRu
        {
            get { return _ValueRu; }
            set { _ValueRu = value; }
        }

        [XmlElement(ElementName = "ValueAz")]
        [DataMember(Name = "ValueAz")]
        public string ValueAz
        {
            get { return _ValueAz; }
            set { _ValueAz = value; }
        }

        public MultiLanguageString()
        {
        }

        public MultiLanguageString(string valueEn, string valueRu, string valueAz)
        {
            _ValueEn = valueEn;
            _ValueRu = valueRu;
            _ValueAz = valueAz;
        }

        public void ReInit()
        {
            if (String.IsNullOrEmpty(_ValueRu) && String.IsNullOrEmpty(_ValueAz))
            {
                _ValueRu = _ValueAz = _ValueEn;
                return;
            }

            if (String.IsNullOrEmpty(_ValueEn) && String.IsNullOrEmpty(_ValueAz))
            {
                _ValueEn = _ValueAz = _ValueRu;
                return;
            }

            if (String.IsNullOrEmpty(_ValueRu) && String.IsNullOrEmpty(_ValueEn))
            {
                _ValueEn = _ValueRu = _ValueAz;
            }
        }

        public override string ToString()
        {
            return _ValueEn;
        }
    }
}
