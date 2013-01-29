using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("User")]
    [DataContract(Name = "User", Namespace = "urn:CashIn")]
    public class User
    {
        private int _Id;
        private String _Username;
        private String _Password;
        private DateTime _InsertDate;
        private DateTime _LastUpdate;
        private Boolean _Active;
        private AccessRole[] _RoleFields;

        [XmlElement(ElementName = "Id")]
        [DataMember(Name = "Id")]
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        [XmlElement(ElementName = "Username")]
        [DataMember(Name = "Username")]
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        [XmlElement(ElementName = "Password")]
        [DataMember(Name = "Password")]
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        [XmlElement(ElementName = "InsertDate")]
        [DataMember(Name = "InsertDate")]
        public DateTime InsertDate
        {
            get { return _InsertDate; }
            set { _InsertDate = value; }
        }

        [XmlElement(ElementName = "LastUpdate")]
        [DataMember(Name = "LastUpdate")]
        public DateTime LastUpdate
        {
            get { return _LastUpdate; }
            set { _LastUpdate = value; }
        }

        [XmlElement(ElementName = "Active")]
        [DataMember(Name = "Active")]
        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }

        [XmlArray("RoleFields")]
        [DataMember(Name = "RoleFields")]
        public AccessRole[] RoleFields
        {
            get { return _RoleFields; }
            set { _RoleFields = value; }
        }

        public User()
        {
        }

        public User(int id, string username, string password, DateTime insertDate, DateTime lastUpdate, bool active, AccessRole[] roleFields)
        {
            _Id = id;
            _Username = username;
            _Password = password;
            _InsertDate = insertDate;
            _LastUpdate = lastUpdate;
            _Active = active;
            _RoleFields = roleFields;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Id: {0}, Username: {1}, Password: {2}, InsertDate: {3}, LastUpdate: {4}, Active: {5}, RoleFields: {6}",
                    _Id, _Username, _Password, _InsertDate, _LastUpdate, _Active, _RoleFields);
        }
    }
}
