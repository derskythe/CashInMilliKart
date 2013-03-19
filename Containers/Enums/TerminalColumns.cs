using System;
using System.Runtime.Serialization;

namespace Containers.Enums
{
    [Serializable]
    [DataContract(Name = "TerminalColumns")]
    public enum TerminalColumns
    {
        [EnumMember]
        Id = 0,
        [EnumMember]
        Name = 1,
        [EnumMember]
        Address = 2,
        [EnumMember]
        IdentityName = 3,
        [EnumMember]
        CreateDate = 4,
        [EnumMember]
        LastUpdate = 5,
        [EnumMember]
        BillsCount = 6,
        [EnumMember]
        StatusType = 7,
        [EnumMember]
        CashcodeStatus = 8,
        [EnumMember]
        StatusUpdate = 9,
        [EnumMember]
        PrinterStatus = 10,
        [EnumMember]
        CashcodeError = 11,
        [EnumMember]
        CashcodeOutStatus = 12,
        [EnumMember]
        CashcodeSuberror = 13,
        [EnumMember]
        PrinterErrorState = 14,
        [EnumMember]
        PrinterExtErrorState = 15,
        [EnumMember]
        Branch = 16
    }
}
