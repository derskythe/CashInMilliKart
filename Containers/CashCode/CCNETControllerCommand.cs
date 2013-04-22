using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Containers.CashCode
{
    [Serializable]
    [DataContract(Name = "CCNETControllerCommand")]
    public enum CCNETControllerCommand
    {
        /// <summary>
        /// Ok
        /// </summary>
        [Description("Ok")]
        [EnumMember]
        Ok = 0x00,                          // Ok

        /// <summary>
        /// Перезагрузка
        /// </summary>
        [Description("Command for Bill-to-Bill unit to self-reset")]
        [EnumMember]
        Reset = 0x30,

        /// <summary>
        /// Получить статус
        /// </summary>
        [Description("Request for Bill-to-Bill unit set-up status")]
        [EnumMember]
        GetStatus = 0x31,                   // получить статус

        /// <summary>
        /// Set security. 
        /// Установить коды безопасности
        /// </summary>
        [Description("Sets Bill-to-Bill unit Security Mode. Command is followed by set-up data")]
        [EnumMember]
        SetSecurity = 0x32,                 // установить коды безопасности

        /// <summary>
        /// POLL. Ping
        /// </summary>
        [Description("POLL. Ping")]
        [EnumMember]
        Poll = 0x33,                         // POLL. Ping,

        /// <summary>
        /// EnableDisable.
        /// Команда на отключение приема вкладов от населения
        /// </summary>
        [Description("Indicates Bill Type enable or disable. ")]
        [EnumMember]
        EnableDisable = 0x34,               // отключить

        /// <summary>
        /// Stack. 
        /// Sent by Controller to stack a bill in escrow to drop cassette or to one of the recycling cassettes
        /// </summary>
        [Description("Sent by Controller to stack a bill in escrow to drop cassette or to one of the recycling cassettes")]
        [EnumMember]
        Stack = 0x35,

        /// <summary>
        /// Return. 
        /// Sent by Controller to return a bill in escrow
        /// </summary>
        [Description("Sent by Controller to return a bill in escrow")]
        [EnumMember]
        Return = 0x36,

        /// <summary>
        /// Identification. 
        /// Request for Model, Serial Number, Country ISO code, Asset Number
        /// </summary>
        [Description("Request for Model, Serial Number, Country ISO code, Asset Number")]
        [EnumMember]
        Identification = 0x37,              // идентификация

        /// <summary>
        /// HoldCommand. 
        /// Command for holding a bill in Escrow state
        /// </summary>
        [Description("Command for holding a bill in Escrow state")]
        [EnumMember]
        HoldCommand = 0x38,

        /// <summary>
        /// Set barcode parameters. 
        /// Command for settings the barcode format and number of characters
        /// </summary>
        [Description("Command for settings the barcode format and number of characters")]
        [EnumMember]
        SetBarcodeParameters = 0x39,

        /// <summary>
        /// Extract barcode data. 
        /// Command for retrieving barcode data if barcode coupon is found
        /// </summary>
        [Description("Command for retrieving barcode data if barcode coupon is found")]
        [EnumMember]
        ExtractBarcodeData = 0x3A,

        /// <summary>
        /// Recycling cassette status. 
        /// Request for Bill-to-Bill unit recycling cassette status
        /// </summary>
        [Description("Request for Bill-to-Bill unit recycling cassette status")]
        [EnumMember]
        RecyclingCassetteStatus = 0x3B,

        /// <summary>
        /// Dispense. 
        /// Command to dispense bill
        /// </summary>
        [Description("Command to dispense bill")]
        [EnumMember]
        Dispense = 0x3C,

        /// <summary>
        /// Unload command.
        /// Command to unload bills from recycling cassettes to drop cassette
        /// </summary>
        [Description("Command to unload bills from recycling cassettes to drop cassette")]
        [EnumMember]
        UnloadCommand = 0x3D,

        /// <summary>
        /// Extended identification.
        /// Request for Model, Serial Number, Software Version of Bill-to-Bill unit and its subunits, Country ISO code, Asset Number
        /// </summary>
        [Description("Request for Model, Serial Number, Software Version of Bill-to-Bill unit and its subunits, Country ISO code, Asset Number")]
        [EnumMember]
        ExtendedIdentification = 0x3E,

        /// <summary>
        /// Set recycling cassette type.
        /// Assigns recycling cassettes to bill type
        /// </summary>
        [Description("Assigns recycling cassettes to bill type")]
        [EnumMember]
        SetRecyclingCassetteType = 0x40,

        /// <summary>
        /// Get bill table.
        /// Request for bill type description
        /// </summary>
        [Description("Request for bill type description")]
        [EnumMember]
        GetBillTable = 0x41,

        /// <summary>
        /// Download. Command for transition to download mode
        /// </summary>
        [Description("Command for transition to download mode")]
        [EnumMember]
        Download = 0x50,

        /// <summary>
        /// Get CRC32 of the code. Request for Bill Validator’s firmware CRC32
        /// </summary>
        [Description("Request for Bill Validator’s firmware CRC32")]
        [EnumMember]
        GetCrc32OfTheCode = 0x51,

        /// <summary>
        /// Module download. Command to enter an internal module update mode
        /// </summary>
        [Description("Command to enter an internal module update mode")]
        [EnumMember]
        ModuleDownload = 0x52,

        /// <summary>
        /// Module identification request. 
        /// Request serial numbers of all intelligent modules
        /// </summary>
        [Description("Request serial numbers of all intelligent modules")]
        [EnumMember]
        ModuleIdentificationRequest = 0x53,

        /// <summary>
        /// Validation module identification. 
        /// Request identification information from banknote validation software module
        /// </summary>
        [Description("Request identification information from banknote validation software module")]
        [EnumMember]
        ValidationModuleIdentification = 0x54,

        /// <summary>
        /// Request statistics. 
        /// Command for retrieving full information about acceptance performance
        /// </summary>
        [Description("Command for retrieving full information about acceptance performance")]
        [EnumMember]
        RequestStatistics = 0x60,

        /// <summary>
        /// Read or initialize internal Real-Time Clock
        /// </summary>
        [Description("Read or initialize internal Real-Time Clock")]
        [EnumMember]
        RealtimeClock = 0x62,

        [Description("Request whether there was a power cut and perform credit recovery")]
        [EnumMember]
        PowerRecovery = 0x66,

        /// <summary>
        /// Empty dispenser. Dispense all bills remaining in the dispenser after power cut
        /// </summary>
        [Description("Dispense all bills remaining in the dispenser after power cut")]
        [EnumMember]
        EmptyDispenser = 0x67,

        /// <summary>
        /// Set options. Set various Bill-To-Bill options
        /// </summary>
        [Description("Set various Bill-To-Bill options")]
        [EnumMember]
        SetOptions = 0x68,

        /// <summary>
        /// Get options. Set various Bill-To-Bill options
        /// </summary>
        [Description("Set various Bill-To-Bill options")]
        [EnumMember]
        GetOptions = 0x69,

        /// <summary>
        /// Extended cassette status. Extended recycling cassette status request
        /// </summary>
        [Description("Extended recycling cassette status request")]
        [EnumMember]
        ExtendedCassetteStatus = 0x70,

        /// <summary>
        /// По нулям
        /// </summary>
        [Description("По нулям")]
        [EnumMember]
        NotMount = 0xFF
    }
}
