using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("LoginResult")]
    [DataContract(Name = "LoginResult", Namespace = "urn:CashIn")]
    public class LoginResult : StandardResult
    {
        private User _UserInfo;
        private String _Sid;

        [XmlElement(ElementName = "Sid")]
        [DataMember(Name = "Sid")]
        public string Sid
        {
            get { return _Sid; }
            set { _Sid = value; }
        }

        [XmlElement(ElementName = "UserInfo")]
        [DataMember(Name = "UserInfo")]
        public User UserInfo
        {
            get { return _UserInfo; }
            set { _UserInfo = value; }
        }

        public LoginResult()
        {
        }

        public LoginResult(ResultCodes code) : base(code)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, UserInfo: {1}", base.ToString(), _UserInfo);
        }
    }
}
