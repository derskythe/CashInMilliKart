using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("ListRolesResult")]
    [DataContract(Name = "ListRolesResult", Namespace = "urn:CashIn")]
    public class ListRolesResult : StandardResult
    {
        private IEnumerable<AccessRole> _Roles;

        [XmlArray("Roles")]
        [DataMember(Name = "Roles")]
        public IEnumerable<AccessRole> Roles
        {
            get { return _Roles; }
            set { _Roles = value; }
        }

        public ListRolesResult(ResultCodes code)
            : base(code)
        {
        }
       
        public ListRolesResult()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Roles: {1}", base.ToString(), EnumEx.GetStringFromArray(_Roles));
        }
    }
}
