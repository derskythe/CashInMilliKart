using CashInTerminal.CashIn;
using Containers.CashCode;

namespace CashInTerminal
{
    public class CCNETDeviceState
    {
        private CCNETCommand _StateCode; // внутр. состояние
        private byte _SubStateCode; // подкод состояния
        private CCNETCommand _StateCodeOut; // внеш. состояние
        private bool _BillEnable; // мы включены и нормально работаем
        private bool _AcceptEnable; // Мы ждем денег
        private bool _FatalError;  // фатальная ошибка. устройство не может работать
        private int _Amount; // Сумма в купюроприемнике
        private int _WasAmount; // Было в купюроприемнике
        private int _Nominal; // номинал последней принятой купюры
        private string _DeviceStateDescription; // описание состояния
        private string _SubDeviceStateDescription; // дополнительное описание состояния
        private bool _Init; // Мы загружены
        private string _Currency;
        private bool _Stacking; // В режиме приема купюры.

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

        public string Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }

        public CashCodeDeviceStatus ToCashCodeDeviceStatus()
        {
            var result = new CashCodeDeviceStatus()
            {
                BillEnable = _BillEnable,
                DeviceStateDescription = _DeviceStateDescription,
                FatalError = _FatalError,
                Init = _Init,
                StateCode = (int)_StateCode,
                StateCodeOut = (int)_StateCodeOut,
                SubDeviceStateDescription = _SubDeviceStateDescription
            };

            return result;
        }
    }
}
