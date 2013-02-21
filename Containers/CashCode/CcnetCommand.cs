using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Containers.CashCode
{
    [Serializable]
    [DataContract(Name = "CCNETCommand")]
    public enum CCNETCommand
    {
        /// <summary>
        /// Ok
        /// </summary>
        [Description("Ok")]
        [EnumMember]
        Ok = 0x00,                          // Ok
        /// <summary>
        /// Ожидание наличности
        /// </summary>
        [Description("Ожидание наличности")]
        [EnumMember]
        Idle = 0x06,                        // Ожидание наличности
        /// <summary>
        /// Не активен
        /// </summary>
        [Description("Не активен")]
        [EnumMember]
        Inactive = 0x01,                    // Не активен
        /// <summary>
        /// готов к транзакциям
        /// </summary>
        [Description("Готов к транзакциям")]
        [EnumMember]
        ReadyForTransaction = 0x07,         // готов к транзакциям
        /// <summary>
        /// Ожидание
        /// </summary>
        [Description("Ожидание")]
        [EnumMember]
        Wait = 0x14,                        // Ожидание
        /// <summary>
        /// Получить статус
        /// </summary>
        [Description("Получить статус")]
        [EnumMember]
        GetStatus = 0x31,                   // получить статус
        /// <summary>
        /// Установить коды безопасности
        /// </summary>
        [Description("Установить коды безопасности")]
        [EnumMember]
        SetSecurity = 0x32,                 // установить коды безопасности
        /// <summary>
        /// Идентификация
        /// </summary>
        [Description("Идентификация")]
        [EnumMember]
        Identification = 0x37,              // идентификация
        /// <summary>
        /// Недоступен
        /// </summary>
        [Description("Недоступен")]
        [EnumMember]
        UnitDisabled = 0x19,                // недоступен
        /// <summary>
        /// Акцептирование
        /// </summary>
        [Description("Акцептирование")]
        [EnumMember]
        Accepting = 0x15,                   // акцептирование
        /// <summary>
        /// Инициализация
        /// </summary>
        [Description("Инициализация")]
        [EnumMember]
        Initialize = 0x13,                  // инициализация
        /// <summary>
        /// Касета полная
        /// </summary>
        [Description("Касета полная")]
        [EnumMember]
        StackerFull = 0x41,                 // Касета полная
        /// <summary>
        /// Вынули кассету
        /// </summary>
        [Description("Вынули кассету")]
        [EnumMember]
        StackerOpened = 0x42,               // Вынули кассету
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
        /// Включили электричество
        /// </summary>
        [Description("Включили электричество")]
        [EnumMember]
        PowerUp = 0x10,                     // Включили электричество
        /// <summary>
        /// Замяло купюру
        /// </summary>
        [Description("Замяло купюру")]
        [EnumMember]
        BillJam = 0x43,                     // Замяло купюру
        /// <summary>
        /// Fatal error
        /// </summary>
        [Description("Fatal Error")]
        [EnumMember]
        FatalError = 0x1C,                  // fatal error
        /// <summary>
        /// Cheated, взлом
        /// </summary>
        [Description("Cheated, взлом")]
        [EnumMember]
        Cheated = 0x45,                     // cheated, взлом
        /// <summary>
        /// Cassete jammed
        /// </summary>
        [Description("Cassete jammed")]
        [EnumMember]
        CasseteBillJam = 0x44,              // cassete jammed
        /// <summary>
        /// Сбой оборудования
        /// </summary>
        [Description("Сбой оборудования")]
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
        [Description("Stacking, вложение купюры в купюроприемник")]
        [EnumMember]
        Stacking = 0x17,                    // stacking, вложение купюры в купюроприемник
        /// <summary>
        /// Stacked, купюра уложена
        /// </summary>
        [Description("Stacked, купюра уложена")]
        [EnumMember]
        Stacked = 0x81,                     // stacked, купюра уложена
        /// <summary>
        /// Возврат купюры
        /// </summary>
        [Description("Возврат купюры")]
        [EnumMember]
        BillReturning = 0x18,                   // возврат купюры
        /// <summary>
        /// Удержание, HOLD
        /// </summary>
        [Description("Удержание, HOLD")]
        [EnumMember]
        Hold = 0x1A,                        // Удержание, HOLD
        /// <summary>
        /// Занят
        /// </summary>
        [Description("Занят")]
        [EnumMember]
        Busy = 0x1B,                        // занят
        /// <summary>
        /// Выдача
        /// </summary>
        [Description("Выдача")]
        [EnumMember]
        Dispensing = 0x1D,                  // Выдача
        /// <summary>
        /// Выгрузка денежного ящика
        /// </summary>
        [Description("Выгрузка денежного ящика")]
        [EnumMember]
        Unloading = 0x1E,                   // выгрузка
        /// <summary>
        /// Установка кассеты
        /// </summary>
        [Description("Установка кассеты")]
        [EnumMember]
        CasseseteInsert = 0x21,             // Установка кассеты
        /// <summary>
        /// "Выдано"
        /// </summary>
        [Description("Выдано")]
        [EnumMember]
        Gone = 0x25,                        // Выдано
        /// <summary>
        /// Разгружено
        /// </summary>
        [Description("Разгружено")]
        [EnumMember]
        Unload = 0x26,                      // Разгружено
        /// <summary>
        /// Неверный код купюры
        /// </summary>
        [Description("Неверный код купюры")]
        [EnumMember]
        UnknownBill = 0x28,                 // Неверный код купюры
        /// <summary>
        /// Кассета установлена
        /// </summary>
        [Description("Кассета установлена")]
        [EnumMember]
        CasseteInsert = 0x29,               // Кассета установлена
        /// <summary>
        /// Кассета переполнена
        /// </summary>
        [Description("Кассета переполнена")]
        [EnumMember]
        CasseteFull = 0x41,                 // Кассета переполнена
        /// <summary>
        /// Кассета снята
        /// </summary>
        [Description("Кассета снята")]
        [EnumMember]
        CasseteRemoved = 0x42,              // Кассета снята
        /// <summary>
        /// Купюра застряла
        /// </summary>
        [Description("Купюра застряла")]
        [EnumMember]
        PaperJam = 0x43,                    // Купюра застряла
        /// <summary>
        /// Купюра застряла
        /// </summary>
        [Description("Купюра застряла")]
        [EnumMember]
        PaperJam2 = 0x44,                   // Купюра застряла
        /// <summary>
        /// Взлом
        /// </summary>
        [Description("Взлом")]
        [EnumMember]
        Hacking = 0x45,                     // Взлом
        /// <summary>
        /// Пауза
        /// </summary>
        [Description("Пауза")]
        [EnumMember]
        Pause = 0x46,                       // Пауза
        /// <summary>
        /// Приём купюры
        /// </summary>
        [Description("Приём купюры")]
        [EnumMember]
        BillReceiving = 0x80,               // Приём купюры
        /// <summary>
        /// Купюра уложена
        /// </summary>
        [Description("Купюра уложена")]
        [EnumMember]
        BillStacked = 0x81,                 // Купюра уложена
        /// <summary>
        /// Купюра Возвращена
        /// </summary>
        [Description("Купюра Возвращена")]
        [EnumMember]
        BillReturned = 0x82,                // Купюра Возвращена
        /// <summary>
        /// Команда на отключение приема вкладов от населения
        /// </summary>
        [Description("Команда на отключение приема вкладов от населения")]
        [EnumMember]
        EnableDisable = 0x34,               // отключить
        /// <summary>
        /// Перезагрузка
        /// </summary>
        [Description("Перезагрузка")]
        [EnumMember]
        Reset = 0x30,                       // Перезагрузка
        /// <summary>
        /// POLL. Ping
        /// </summary>
        [Description("POLL. Ping")]
        [EnumMember]
        Poll = 0x33                         // POLL. Ping
    }
}
