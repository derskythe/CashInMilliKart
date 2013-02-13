using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("UserSession")]
    [DataContract(Name = "UserSession", Namespace = "urn:CashIn")]
    public class UserSession
    {
        private string _Sid;
        private DateTime _StartDate;
        private DateTime _LastUpdate;
        private User _User;

        [XmlElement(ElementName = "Sid")]
        [DataMember(Name = "Sid")]
        public string Sid
        {
            get { return _Sid; }
            set { _Sid = value; }
        }

        [XmlElement(ElementName = "StartDate")]
        [DataMember(Name = "StartDate")]
        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }

        [XmlElement(ElementName = "LastUpdate")]
        [DataMember(Name = "LastUpdate")]
        public DateTime LastUpdate
        {
            get { return _LastUpdate; }
            set { _LastUpdate = value; }
        }

        [XmlElement(ElementName = "User")]
        [DataMember(Name = "User")]
        public User User
        {
            get { return _User; }
            set { _User = value; }
        }

        public UserSession()
        {
        }

        public UserSession(string sid, DateTime startDate, DateTime lastUpdate, User user)
        {
            _Sid = sid;
            _StartDate = startDate;
            _LastUpdate = lastUpdate;
            _User = user;
        }

        public override string ToString()
        {
            return string.Format("Sid: {0}, StartDate: {1}, LastUpdate: {2}, User: {3}", _Sid, _StartDate, _LastUpdate,
                                 _User);
        }
    }
}
