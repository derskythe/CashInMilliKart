using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

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
