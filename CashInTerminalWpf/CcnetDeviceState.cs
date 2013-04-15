using Containers.CashCode;

namespace CashInTerminalWpf
{
    public class CcnetDeviceState
    {
        private CCNETCommand _StateCode; // внутр. состояние
        /// <summary>
        /// Внутр. состояние
        /// </summary>
        public CCNETCommand StateCode
        {
            get
            {
                return _StateCode;
            }
            set
            {
                _StateCode = value;
            }
        }
        private byte _SubStateCode; // подкод состояния
        /// <summary>
        /// Подкод состояния
        /// </summary>
        public byte SubStateCode
        {
            get
            {
                return _SubStateCode;
            }
            set
            {
                _SubStateCode = value;
            }
        }
        private CCNETCommand _StateCodeOut; // внеш. состояние

        /// <summary>
        /// внеш. состояние
        /// </summary>
        public CCNETCommand StateCodeOut
        {
            get
            {
                return _StateCodeOut;
            }
            set
            {
                _StateCodeOut = value;
            }
        }
        private bool _BillEnable; // мы включены и нормально работаем

        /// <summary>
        /// мы включены и нормально работаем
        /// </summary>
        public bool BillEnable
        {
            get
            {
                return _BillEnable;
            }
            set
            {
                _BillEnable = value;
            }
        }
        private bool _AcceptEnable; // Мы ждем денег

        /// <summary>
        /// Мы ждем денег
        /// </summary>
        public bool AcceptEnable
        {
            get
            {
                return _AcceptEnable;
            }
            set
            {
                _AcceptEnable = value;
            }
        }
        private bool _FatalError;  // фатальная ошибка. устройство не может работать

        /// <summary>
        /// фатальная ошибка. устройство не может работать
        /// </summary>
        public bool FatalError
        {
            get
            {
                return _FatalError;
            }
            set
            {
                _FatalError = value;
            }
        }
        private int _Amount; // Сумма в купюроприемнике

        /// <summary>
        /// Сумма в купюроприемнике
        /// </summary>
        public int Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                _Amount = value;
            }
        }
        private int _WasAmount; // Было в купюроприемнике

        /// <summary>
        /// Было в купюроприемнике
        /// </summary>
        public int WasAmount
        {
            get
            {
                return _WasAmount;
            }
            set
            {
                _WasAmount = value;
            }
        }
        private int _Nominal; // номинал последней принятой купюры

        /// <summary>
        /// номинал последней принятой купюры
        /// </summary>
        public int Nominal
        {
            get
            {
                return _Nominal;
            }
            set
            {
                _Nominal = value;
            }
        }
        private string _DeviceStateDescription; // описание состояния

        /// <summary>
        /// описание состояния
        /// </summary>
        public string DeviceStateDescription
        {
            get
            {
                return _DeviceStateDescription;
            }
            set
            {
                _DeviceStateDescription = value;
            }
        }
        private string _SubDeviceStateDescription; // дополнительное описание состояния

        /// <summary>
        /// дополнительное описание состояния
        /// </summary>
        public string SubDeviceStateDescription
        {
            get
            {
                return _SubDeviceStateDescription;
            }
            set
            {
                _SubDeviceStateDescription = value;
            }
        }
        private bool _Stacking; // В режиме приема купюры.


        /// <summary>
        /// В режиме приема купюры
        /// </summary>
        public bool Stacking
        {
            get
            {
                return _Stacking;
            }
            set
            {
                _Stacking = value;
            }
        }
        private bool _Init; // Мы загружены

        /// <summary>
        /// Мы загружены
        /// </summary>
        public bool Init
        {
            get
            {
                return _Init;
            }
            set
            {
                _Init = value;
            }
        }
    }
}
