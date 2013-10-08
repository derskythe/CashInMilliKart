using System;
using System.Runtime.Serialization;

namespace Containers.Enums
{
    [Serializable]
    [DataContract(Name = "GetClientInfoType")]
    public enum GetClientInfoType
    {
        [EnumMember]
        CreditByClientCode = 11,
        [EnumMember]
        CreditByPasportAndCreditNumber = 12,
        [EnumMember]
        Bolcard = 13,
        [EnumMember]
        DebitByClientCode = 21,
        [EnumMember]
        DebitByPasportAndCreditNumber = 22
    }
}
