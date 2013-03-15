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
        ByClientCode = 1,
        [EnumMember]
        ByPasportAndCreditNumber = 2,
        [EnumMember]
        Bolcard = 3
    }
}
