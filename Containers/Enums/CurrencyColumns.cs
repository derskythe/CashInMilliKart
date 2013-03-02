using System;
using System.Runtime.Serialization;

namespace Containers.Enums
{
    [Serializable]
    [DataContract(Name = "CurrencyColumns")]
    public enum CurrencyColumns
    {
        [EnumMember]
        Id = 0,
        [EnumMember]
        Name = 1,
        [EnumMember]
        IsoName = 2,
        [EnumMember]
        Rate = 3
    }
}
