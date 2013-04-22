using System.ComponentModel;
using System.Runtime.Serialization;

namespace Containers.CashCode
{
    public enum CCNETErrorCodes
    {
        /// <summary>
        /// Ok
        /// </summary>
        [Description("NoErrors")]
        [EnumMember]
        NoErrors = 0x00,

        [Description("Stack Motor Failure. Drop Cassette Motor failure")]
        [EnumMember]
        StackMotorFaillure = 0x50,

        [Description("Transport Motor Speed Failure")]
        [EnumMember]
        TransportMotorSpeedFailure = 0x51,

        [Description("Transport Motor Failure")]
        [EnumMember]
        TransportMotorFailure = 0x52,

        [Description("Aligning Motor Failure")]
        [EnumMember]
        AligningMotorFailure = 0x53,

        [Description("Initial Cassette Status Failure")]
        [EnumMember]
        InitialCassetteStatusFailure = 0x54,


        [Description("Optic Canal Failure. One of the optic sensors has failed to provide its response")]
        [EnumMember]
        OpticCanalFailure = 0x55,


        [Description("Magnetic Canal Failure. Inductive sensor failed to respond")]
        [EnumMember]
        MagneticCanalFailure  = 0x56,


        [Description("Capacitance Canal Failure. Capacitance sensor failed to respond")]
        [EnumMember]
        CapacitanceCanalFailure  = 0x5F       
    }
}
