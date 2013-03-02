using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Containers.Enums
{
    [Serializable]
    [DataContract(Name = "SortType")]
    public enum SortType
    {
        [EnumMember]
        Asc = 0,
        [EnumMember]
        Desc = 1
    }
}
