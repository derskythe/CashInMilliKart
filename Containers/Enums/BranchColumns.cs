using System;
using System.Runtime.Serialization;

namespace Containers.Enums
{
    [Serializable]
    [DataContract(Name = "BranchColumns")]
    public enum BranchColumns
    {
        [EnumMember]
        Name = 0,
        [EnumMember]
        UserName = 1,
        [EnumMember]
        InsertDate = 2,
        [EnumMember]
        UpdateDate = 3
    }
}
