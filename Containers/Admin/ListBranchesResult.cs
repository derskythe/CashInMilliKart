using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("ListBranchesResult")]
    [DataContract(Name = "ListBranchesResult", Namespace = "urn:CashIn")]
    public class ListBranchesResult : StandardResult
    {
        private List<Branch> _Branches;

        public ListBranchesResult(ResultCodes code) : base(code)
        {
        }

        [XmlArray("Branches")]
        [DataMember(Name = "Branches")]
        public List<Branch> Branches
        {
            get { return _Branches; }
            set { _Branches = value; }
        }

        public ListBranchesResult()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Branches: {1}", base.ToString(), EnumEx.GetStringFromArray(_Branches));
        }
    }
}
