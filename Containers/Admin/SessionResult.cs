using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("SessionResult")]
    [DataContract(Name = "SessionResult", Namespace = "urn:CashIn")]
    public class SessionResult : StandardResult
    {
        private UserSession _Session;

        [XmlElement(ElementName = "Session")]
        [DataMember(Name = "Session")]
        public UserSession Session
        {
            get { return _Session; }
            set { _Session = value; }
        }

        public SessionResult()
        {
        }

        public SessionResult(ResultCodes code)
            : base(code)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Session: {1}", base.ToString(), _Session);
        }
    }
}
