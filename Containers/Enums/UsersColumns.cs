using System;
using System.Runtime.Serialization;

namespace Containers.Enums
{
    [Serializable]
    [DataContract(Name = "UsersColumns")]
    public enum UsersColumns
    {
        [EnumMember]
        Username = 0,
        [EnumMember]
        InsertDate = 1,
        [EnumMember]
        UpdateDate = 2
    }
}
