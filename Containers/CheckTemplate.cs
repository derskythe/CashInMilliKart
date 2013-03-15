using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("CheckTemplate")]
    [DataContract(Name = "CheckTemplate", Namespace = "urn:CashIn")]
    public class CheckTemplate
    {
        private int _Id;
        private CheckType _CheckType;
        private string _Language;
        private bool _Active;
        private DateTime _InsertDate;
        private DateTime _UpdateDate;
        private List<CheckField> _Fields;

        [XmlElement(ElementName = "Id")]
        [DataMember(Name = "Id")]
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        [XmlElement(ElementName = "CheckType")]
        [DataMember(Name = "CheckType")]
        public CheckType CheckType
        {
            get { return _CheckType; }
            set { _CheckType = value; }
        }

        [XmlElement(ElementName = "Language")]
        [DataMember(Name = "Language")]
        public string Language
        {
            get { return _Language; }
            set { _Language = value; }
        }

        [XmlElement(ElementName = "Active")]
        [DataMember(Name = "Active")]
        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }

        [XmlElement(ElementName = "InsertDate")]
        [DataMember(Name = "InsertDate")]
        public DateTime InsertDate
        {
            get { return _InsertDate; }
            set { _InsertDate = value; }
        }

        [XmlElement(ElementName = "UpdateDate")]
        [DataMember(Name = "UpdateDate")]
        public DateTime UpdateDate
        {
            get { return _UpdateDate; }
            set { _UpdateDate = value; }
        }

        [XmlArray("Fields")]
        [DataMember(Name = "Fields")]
        public List<CheckField> Fields
        {
            get { return _Fields; }
            set { _Fields = value; }
        }

        public CheckTemplate()
        {
        }

        public CheckTemplate(int id, CheckType checkType, string language, bool active, DateTime insertDate, DateTime updateDate, List<CheckField> fields)
        {
            _Id = id;
            _CheckType = checkType;
            _Language = language;
            _Active = active;
            _InsertDate = insertDate;
            _UpdateDate = updateDate;
            _Fields = fields;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Id: {0}, CheckType: {1}, Language: {2}, Active: {3}, InsertDate: {4}, UpdateDate: {5}, Fields: {6}",
                    _Id, _CheckType, _Language, _Active, _InsertDate, _UpdateDate, EnumEx.GetStringFromArray(_Fields));
        }
    }
}
