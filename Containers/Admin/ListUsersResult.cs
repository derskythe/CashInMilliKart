using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("ListUsersResult")]
    [DataContract(Name = "ListUsersResult", Namespace = "urn:CashIn")]
    public class ListUsersResult : StandardResult
    {
        private User[] _Users;

        [XmlArray("Users")]
        [DataMember(Name = "Users")]
        public User[] Users
        {
            get { return _Users; }
            set { _Users = value; }
        }

        public ListUsersResult()
        {
        }

        public ListUsersResult(ResultCodes code)
            : base(code)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Users: {1}", base.ToString(), EnumEx.GetStringFromArray(_Users));
        }
    }
}
