using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("Branch")]
    [DataContract(Name = "Branch", Namespace = "urn:CashIn")]
    public class Branch
    {
        private int _Id;
        private string _Name;
        private int _UserId;
        private DateTime _InsertDate;
        private DateTime _UpdateDate;
        private String _Username;

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

        [XmlElement(ElementName = "UserId")]
        [DataMember(Name = "UserId")]
        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
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

        [XmlElement(ElementName = "Username")]
        [DataMember(Name = "Username")]
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        public Branch()
        {
        }

        public Branch(int id, string name, int userId, DateTime insertDate, DateTime updateDate, string username)
        {
            _Id = id;
            _Name = name;
            _UserId = userId;
            _InsertDate = insertDate;
            _UpdateDate = updateDate;
            _Username = username;
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, UserId: {2}, InsertDate: {3}, UpdateDate: {4}, Username: {5}", _Id,
                                 _Name, _UserId, _InsertDate, _UpdateDate, _Username);
        }
    }
}
