using System;
using System.Collections.Generic;
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
        private String _Salt;
        private DateTime _InsertDate;
        private DateTime _LastUpdate;
        private Boolean _Active;
        private List<AccessRole> _RoleFields;
        private List<Branch> _Branches;

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

        [XmlElement(ElementName = "Salt")]
        [DataMember(Name = "Salt")]
        public string Salt
        {
            get { return _Salt; }
            set { _Salt = value; }
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
        public List<AccessRole> RoleFields
        {
            get { return _RoleFields; }
            set { _RoleFields = value; }
        }

        [XmlArray("Branches")]
        [DataMember(Name = "Branches")]
        public List<Branch> Branches
        {
            get { return _Branches; }
            set { _Branches = value; }
        }

        public User()
        {
        }

        public User(int id, string username, string password, string salt, DateTime insertDate, DateTime lastUpdate, bool active, List<AccessRole> roleFields)
        {
            _Id = id;
            _Username = username;
            _Password = password;
            _Salt = salt;
            _InsertDate = insertDate;
            _LastUpdate = lastUpdate;
            _Active = active;
            _RoleFields = roleFields;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Id: {0}, Username: {1}, Password: {2}, Salt: {3}, InsertDate: {4}, LastUpdate: {5}, Active: {6}, RoleFields: {7}, Branches: {8}",
                    _Id, _Username, _Password, _Salt, _InsertDate, _LastUpdate, _Active, EnumEx.GetStringFromArray(_RoleFields), EnumEx.GetStringFromArray(_Branches));
        }
    }
}
