using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Containers.CashCode
{
    [Serializable]
    [DataContract(Name = "CCNETRejectReason")]
    public enum CCNETRejectReason
    {
        [Description("None")]
        [EnumMember]
        None = 0x00,

        [Description("Rejecting due to Insertion. Insertion error")]
        [EnumMember]
        InsertionError = 0x60,

        [Description("Rejecting due to Magnetic. Magnetic error")]
        [EnumMember]
        MagneticError = 0x61,

        [Description("Rejecting due to bill Remaining in the head. Bill remains in the head, and new bill is rejected")]
        [EnumMember]
        RemainingInHead = 0x62,

        [Description("Rejecting due to Multiplying. Compensation error/multiplying factor error")]
        [EnumMember]
        Multiplying = 0x63,

        [Description("Rejecting due to Conveying. Conveying error")]
        [EnumMember]
        Conveying = 0x64,

        [Description("Rejecting due to Identification1. Identification error")]
        [EnumMember]
        Identification = 0x65,

        [Description("Rejecting due to Verification. Verification error")]
        [EnumMember]
        Verification = 0x66,

        [Description("Rejecting due to Optic. Optic error")]
        [EnumMember]
        OpticError = 0x67,

        [Description("Rejecting due to Inhibit. Returning by inhibit denomination error")]
        [EnumMember]
        InhibitDenomination = 0x68,

        [Description("Rejecting due to Capacity. Capacitance error")]
        [EnumMember]
        Capacity = 0x69,

        [Description("Rejecting due to Operation. Operation error")]
        [EnumMember]
        Operation = 0x6A,

        [Description("Rejecting due to Length. Length error")]
        [EnumMember]
        Length = 0x6C,

        [Description("Rejecting due to UV optic. Banknote UV properties do not meet the predefined criteria")]
        [EnumMember]
        BanknoteUv = 0x6D,

        [Description("Rejecting due to unrecognised barcode. Bill taken was treated as a barcode but no reliable data can be read from it")]
        [EnumMember]
        UnrecognisedBarcode = 0x92,

        [Description("Rejecting due to incorrect number of characters in barcode. Barcode data was read (at list partially) but is inconsistent")]
        [EnumMember]
        IncorrectNumberCharacters = 0x93,

        [Description("Rejecting due to unknown barcode start sequence. Barcode was not read as no synchronization was established")]
        [EnumMember]
        UnknownBarcodeStartSequence = 0x94,

        [Description("Rejecting due to unknown barcode stop sequence. Barcode was read but trailing data is corrupt")]
        [EnumMember]
        UnknownBarcodeStopSequence = 0x95
    }
}
