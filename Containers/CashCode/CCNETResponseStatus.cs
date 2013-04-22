using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Containers.CashCode
{
    [Serializable]
    [DataContract(Name = "CCNETResponseStatus")]
    public enum CCNETResponseStatus
    {
        /// <summary>
        /// Ok
        /// </summary>
        [Description("Ok")]
        [EnumMember]
        Ok = 0x00,                          // Ok

        /// <summary>
        /// Inactive. Indicates the Card Reader has been reset/re-powered
        /// </summary>
        [Description("Inactive. Indicates the Card Reader has been reset/re-powered")]
        [EnumMember]
        Inactive = 0x01,                    // Не активен

        /// <summary>
        /// Disabled
        /// </summary>
        [Description("Disabled. Indicates the Card Reader has received all SETUP information from the Controller")]
        [EnumMember]
        Disabled = 0x05,

        /// <summary>
        /// Ожидание наличности
        /// </summary>
        [Description("Idling")]
        [EnumMember]
        Idling = 0x06,                        // Ожидание наличности
        

        /// <summary>
        /// Ready for transaction. Indicates the Card Reader is available for vending activity
        /// </summary>
        [Description("Ready for transaction. Indicates the Card Reader is available for vending activity")]
        [EnumMember]
        ReadyForTransaction = 0x07,         // готов к транзакциям
        
        /// <summary>
        /// Wait. Ready accept bills
        /// </summary>
        [Description("Wait. Ready accept bills")]
        [EnumMember]
        Wait = 0x14,                        // Ожидание
                       
        /// <summary>
        /// UnitDisabled
        /// </summary>
        [Description("UnitDisabled")]
        [EnumMember]
        UnitDisabled = 0x19,                // недоступен

        /// <summary>
        /// Accepting
        /// </summary>
        [Description("Accepting")]
        [EnumMember]
        Accepting = 0x15,                   // акцептирование

        /// <summary>
        /// Initializes
        /// </summary>
        [Description("Initializes")]
        [EnumMember]
        Initialize = 0x13,                  // инициализация                

        /// <summary>
        /// Включили электричество with BILL
        /// </summary>
        [Description("Включили электричество with BILL")]
        [EnumMember]
        PowerUpWithBillInAcceptor = 0x11,   // Включили электричество with BILL
        
        /// <summary>
        /// Включили электричество with BILL
        /// </summary>
        [Description("Включили электричество with BILL")]
        [EnumMember]
        PowerUpWithBillInStacker = 0x12,    // Включили электричество with BILL

        /// <summary>
        /// PowerUp. The state of a B2B after a power up
        /// </summary>
        [Description("PowerUp. The state of a B2B after a power up")]
        [EnumMember]
        PowerUp = 0x10,                     // Включили электричество
        

        /// <summary>
        /// Fatal error
        /// </summary>
        [Description("Rejecting")]
        [EnumMember]
        Rejecting = 0x1C,                  // fatal error             

        /// <summary>
        /// Generic error
        /// </summary>
        [Description("Generic error")]
        [EnumMember]
        Error = 0x47,                       // сбой оборудования

        /// <summary>
        /// По нулям
        /// </summary>
        [Description("По нулям")]
        [EnumMember]
        NotMount = 0xFF,                    // по нулям

        /// <summary>
        /// Прием вкладов от населения
        /// </summary>
        [Description("Прием вкладов от населения")]
        [EnumMember]
        BillAccepting = 0x80,               // прием вкладов от населения

        /// <summary>
        /// Stacking, вложение купюры в купюроприемник
        /// </summary>
        [Description("Stacking")]
        [EnumMember]
        Stacking = 0x17,                    // stacking, вложение купюры в купюроприемник        

        /// <summary>
        /// Возврат купюры
        /// </summary>
        [Description("Returning")]
        [EnumMember]
        BillReturning = 0x18,                   // возврат купюры

        /// <summary>
        /// Hold. The state, in which the bill is held in Escrow position after the HOLD command from the Controller
        /// </summary>
        [Description("Hold. The state, in which the bill is held in Escrow position after the HOLD command from the Controller")]
        [EnumMember]
        Holding = 0x1A,                        // Удержание, HOLD

        /// <summary>
        /// Занят
        /// </summary>
        [Description("Busy")]
        [EnumMember]
        Busy = 0x1B,                        // занят

        /// <summary>
        /// Dispensing. 
        /// B2B moves the bill(s) from recycling cassette to dispenser
        /// </summary>
        [Description("Dispensing")]
        [EnumMember]
        Dispensing = 0x1D,                  // Выдач

        /// <summary>
        /// Unloading. 
        /// B2B is moving the bill(s) from recycling cassette to drop cassette. 
        /// Number of bills requested is more than the number of bills in the cassette
        /// </summary>
        [Description("Unloading")]
        [EnumMember]
        Unloading = 0x1E,                   // выгрузка

        /// <summary>
        /// Cassette insert. 
        /// The unloading of the recycling cassette is carried out, 
        /// and if it is necessary, reprogramming EEPROM
        /// </summary>
        [Description("Cassette insert. " +
                     "The unloading of the recycling cassette is carried out, and if it is necessary, reprogramming EEPROM")]
        [EnumMember]
        CassetteInsert = 0x21,             // Установка кассеты

        /// <summary>
        /// Dispensed. Dispensing is completed
        /// </summary>
        [Description("Dispensed")]
        [EnumMember]
        Dispensed = 0x25,                        // Выдано

        /// <summary>
        /// Unloaded. Unloading is completed
        /// </summary>
        [Description("Unloaded")]
        [EnumMember]
        Unload = 0x26,                      // Разгружено

        /// <summary>
        /// Invalid bill number. Required number of bills is incorrect
        /// </summary>
        [Description("Invalid bill number. Required number of bills is incorrect")]
        [EnumMember]
        InvalidBillNumber = 0x28,                 // Неверный код купюры

        /// <summary>
        /// Set Cassette Type. Setting recycling cassette type is completed
        /// </summary>
        [Description("Set Cassette Type. Setting recycling cassette type is completed")]
        [EnumMember]
        SetCassetteType = 0x29,               // Кассета установлена

        /// <summary>
        /// Invalid command. 
        /// Command from the Controller is not valid
        /// </summary>
        [Description("Command from the Controller is not valid")]
        [EnumMember]
        InvalidCommand = 0x30,
        /// <summary>
        /// Cassete full. Drop Cassette full condition
        /// </summary>
        [Description("Drop cassette full condition")]
        [EnumMember]
        CassetteFull = 0x41,                 // Кассета переполнена

        /// <summary>
        /// Cassette removed. The B2B unit has detected the drop cassette to be open or removed
        /// </summary>
        [Description("Cassette removed. The B2B unit has detected the drop cassette to be open or removed")]
        [EnumMember]
        CassetteRemoved = 0x42,              // Кассета снята
        
        /// <summary>
        /// Jam in acceptor. A bill has jammed in the bill path
        /// </summary>
        [Description("Jam in acceptor. A bill has jammed in the bill path")]
        [EnumMember]
        JamInAcceptor = 0x43,                    // Купюра застряла
        /// <summary>
        /// Jam in stacker. A bill has jammed in drop cassette
        /// </summary>
        [Description("Jam in stacker. A bill has jammed in drop cassette")]
        [EnumMember]
        JamInStacker = 0x44,                   // Купюра застряла

        /// <summary>
        /// Cheated. The Bill-to-Bill unit detected attempts by to user to cheat
        /// </summary>
        [Description("Cheated. The Bill-to-Bill unit detected attempts by to user to cheat")]
        [EnumMember]
        Cheated = 0x45,                     // Взлом

        /// <summary>
        /// Pause. When the user tries to insert a second bill when the previous bill is in the Bill Validator buthas not been stacked
        /// </summary>
        [Description("Pause. When the user tries to insert a second bill when the previous bill is in the Bill Validator buthas not been stacked")]
        [EnumMember]
        Pause = 0x46,                       // Пауза

        /// <summary>
        /// Приём купюры
        /// </summary>
        [Description("Bill receiving")]
        [EnumMember]
        BillReceiving = 0x80,               // Приём купюры

        /// <summary>
        /// Купюра уложена
        /// </summary>
        [Description("Bill stacked")]
        [EnumMember]
        BillStacked = 0x81,                 // Купюра уложена

        /// <summary>
        /// Returned
        /// </summary>
        [Description("Returned")]
        [EnumMember]
        BillReturned = 0x82,                // Купюра Возвращена                                    
    }
}
