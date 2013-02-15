using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Containers.Enums
{
    [Serializable]
    [DataContract(Name = "ResultCodes")]
    public enum ResultCodes
    {
        [Description("OK")]
        [EnumMember]
        Ok = 0,

        [Description("Invalid number")]
        [EnumMember]
        InvalidNumber = 1,

        [Description("Invalid parameters")]
        [EnumMember]
        InvalidParameters = 2,

        [Description("Invalid terminal")]
        [EnumMember]
        InvalidTerminal = 3,

        [Description("Invalid terminal key")]
        [EnumMember]
        InvalidKey = 4,

        [Description("Invalid signature of message")]
        [EnumMember]
        InvalidSignature = 5,

        [Description("Invalid username or password")]
        [EnumMember]
        InvalidUsernameOrPassword = 6,

        [Description("Invalid session")]
        [EnumMember]
        InvalidSession = 7,

        [Description("Insufficient privileges")]
        [EnumMember]
        NoPriv = 8,

        [Description("Unknown Error")]
        [EnumMember]
        UnknownError = 128,

        [Description("System Error")]
        [EnumMember]
        SystemError = 256
    }
}
