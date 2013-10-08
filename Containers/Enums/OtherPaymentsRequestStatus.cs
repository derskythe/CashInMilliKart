using System;
using System.Runtime.Serialization;

namespace Containers.Enums
{
    [Serializable]
    [DataContract(Name = "OtherPaymentsRequestStatus")]
    public enum OtherPaymentsRequestStatus
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        Done = 1,
        [EnumMember]
        Requested = 2
    }
}
