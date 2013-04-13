using System;
using System.Runtime.Serialization;

namespace Containers.Enums
{
    [Serializable]
    [DataContract(Name = "ProductHistoryColumns")]
    public enum ProductHistoryColumns
    {
        [EnumMember]
        TransactionId = 0,
        [EnumMember]
        TerminalId = 1,
        [EnumMember]
        TerminalIdentityName = 2,
        [EnumMember]
        TerminalName = 3,
        [EnumMember]
        TerminalAddress = 4,
        [EnumMember]
        ProductId = 5,
        [EnumMember]
        CurrencyId = 6,
        [EnumMember]
        CurrencyRate = 7,
        [EnumMember]
        InsertDate = 8,
        [EnumMember]
        Amount = 9,
        [EnumMember]
        ProductName = 10,
        [EnumMember]
        Encashment = 11
    }
}
