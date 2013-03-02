using System;
using System.Runtime.Serialization;

namespace Containers.Enums
{
    [Serializable]
    [DataContract(Name = "EncashmentColumns")]
    public enum EncashmentColumns
    {
        [EnumMember]
        TerminalId = 0,
        [EnumMember]
        TerminalName = 1,
        [EnumMember]
        Username = 2,
        [EnumMember]
        InsertDate = 3
    }
}
