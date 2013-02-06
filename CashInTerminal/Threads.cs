using System;
using System.Collections.Generic;
using System.Threading;
using CashInTerminal.CashIn;
using CashInTerminal.Properties;
using Containers.Enums;
using ResultCodes = CashInTerminal.CashIn.ResultCodes;

namespace CashInTerminal
{
    public partial class FormMain
    {
        private Thread _PingThread;
        private Thread _SendPaymentThread;
        private Timer _CheckCurrencyTimer;
        private Timer _CheckProductsTimer;
        private Timer _CheckInactivityTimer;
        private const int PING_TIMEOUT = 30 * 1000;
        private const int SEND_PAYMENT_TIMEOUT = 10000;
        private const int CHECK_CURRENCY_TIMER = 30 * 60 * 1000;
        private const int CHECK_PRODUCTS_TIMER = 60 * 60 * 1000;
        private const int CHECK_INACTIVITY = 10 * 1000;
        private const int MAX_INACTIVITY_PERIOD = 2 * 60; // Seconds!
        private readonly List<Currency> _Currencies = new List<Currency>();
        private readonly List<Product> _Products = new List<Product>();

        private uint _LastActivity;

        public delegate void CheckCurrencyTimerCaller(object param);
        public delegate void CheckProductsTimerCaller(object param);

        private void CheckInactivityTimer(object param)
        {
            //Log.Debug(Utilities.GetLastInputTime());
            if (_SelectedPanel != "pnlMoney" && _SelectedPanel != "pnlLanguage" && _SelectedPanel != "pnlTestMode" && _SelectedPanel != "pnlEncashment")
            {
                _LastActivity = Utilities.GetLastInputTime();
                if (_LastActivity > MAX_INACTIVITY_PERIOD)
                {
                    ChangePannel(pnlLanguage);
                }
            }
            else
            {
                _LastActivity = 0;
            }
        }

        private void CheckCurrencyTimer(object param)
        {
            if (_Running && _Init && _AuthTerminal)
            {
                try
                {
                    var now = DateTime.Now;
                    var request = new StandardRequest
                        {
                            SystemTime = now,
                            TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                            Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey)
                        };

                    var response = _Server.ListCurrencies(request);

                    CheckSignature(response);

                    if (response.ResultCodes == ResultCodes.Ok)
                    {
                        lock (_Currencies)
                        {
                            _Currencies.Clear();
                            foreach (var item in response.Currencies)
                            {
                                _Currencies.Add(item);
                            }
                        }
                    }
                    else
                    {
                        Log.Warn(response.ResultCodes + " " + response.Description);
                    }
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                }
            }
        }

        private void CheckProductsTimer(object param)
        {
            if (_Running && _Init && _AuthTerminal)
            {
                try
                {
                    var now = DateTime.Now;
                    var request = new StandardRequest
                    {
                        SystemTime = now,
                        TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                        Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey)
                    };

                    var response = _Server.ListProducts(request);

                    CheckSignature(response);

                    if (response.ResultCodes == ResultCodes.Ok)
                    {
                        lock (_Products)
                        {
                            _Products.Clear();
                            foreach (var item in response.Products)
                            {
                                _Products.Add(item);
                            }
                        }
                    }
                    else
                    {
                        Log.Warn(response.ResultCodes + " " + response.Description);
                    }
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                }
            }
        }

        private void PingThread()
        {
            while (_Running)
            {
                while (_Init && _AuthTerminal)
                {
                    try
                    {
                        var now = DateTime.Now;
                        var request = new PingRequest
                            {
                                CashCodeStatus = (int)_CcnetDevice.DeviceState.StateCode,
                                SystemTime = now,
                                TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                                TerminalStatus = !_EncashmentMode ? (int)TerminalCodes.Ok : (int)TerminalCodes.Encashment,
                                Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey)
                            };
                        var result = _Server.Ping(request);

                        CheckSignature(result);

                        if (result != null)
                        {
                            if (result.ResultCodes != ResultCodes.Ok)
                            {
                                Log.Warn(String.Format("Server return: {0}, {1}", result.ResultCodes, result.Description));
                            }
                            else
                            {
                                switch ((TerminalCommands)result.Command)
                                {
                                    case TerminalCommands.OutOfService:
                                        Log.Warn("Received command " + TerminalCommands.OutOfService.ToString());

                                        if (_SelectedPanel != "pnlMoney" && _SelectedPanel != "pnlPaySuccess")
                                        {
                                            ChangePannel(pnlOutOfOrder);
                                            CommandReceived();
                                        }
                                        break;

                                    case TerminalCommands.TestMode:
                                        Log.Warn("Received command " + TerminalCommands.TestMode.ToString());
                                        if (_SelectedPanel != "pnlMoney" && _SelectedPanel != "pnlPaySuccess")
                                        {
                                            ChangePannel(pnlTestMode);
                                            CommandReceived();
                                        }
                                        break;

                                    case TerminalCommands.Encash:
                                        Log.Warn("Received command " + TerminalCommands.Encash.ToString());

                                        if (_SelectedPanel != "pnlMoney" && _SelectedPanel != "pnlPaySuccess")
                                        {
                                            DoEncashment();
                                        }
                                        break;

                                    case TerminalCommands.Idle:
                                    case TerminalCommands.NormalMode:
                                        if ((_SelectedPanel == "pnlOutOfOrder" || _SelectedPanel == "pnlTestMode"))
                                        {
                                            GetTerminalInfo();
                                            ChangePannel(pnlLanguage);
                                        }

                                        CommandReceived();
                                        break;
                                }
                                // TODO : Добавить обработчик этого
                            }
                        }
                        else
                        {
                            Log.Error("Result is null");
                        }
                    }
                    catch (Exception exp)
                    {
                        Log.ErrorException(exp.Message, exp);
                    }
                    Thread.Sleep(PING_TIMEOUT);
                }

                Thread.Sleep(PING_TIMEOUT);
            }
        }

        private void GetTerminalInfo()
        {
            DateTime now = DateTime.Now;
            var cmd = new StandardRequest
                {
                    TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                    SystemTime = now,
                    Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey)
                };
            var response = _Server.GetTerminalInfo(cmd);

            if (response != null)
            {
                _TerminalInfo = response.Terminal;
            }
        }

        private StandardResult CommandReceived()
        {
            DateTime now = DateTime.Now;
            var cmd = new StandardRequest
                {
                    TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                    SystemTime = now,
                    Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey)
                };
            return _Server.CommandReceived(cmd);
        }

        private void DoEncashment()
        {
            ChangePannel(pnlEncashment);

            try
            {
                var secondResult = CommandReceived();
                CheckSignature(secondResult);

                var request = new Encashment();

                var currList = new List<EncashmentCurrency>();
                foreach (var currency in _Currencies)
                {
                    var total = _Db.GetCasseteTotal(currency.Name);

                    var item = new EncashmentCurrency {Amount = total, Currency = currency.Name};
                    currList.Add(item);
                }

                var now = DateTime.Now;
                request.SystemTime = now;
                request.TerminalId = Convert.ToInt32(Settings.Default.TerminalCode);
                request.Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey);

                request.Currencies = currList.ToArray();

                var result = _Server.Encashment(request);

                if (result != null)
                {
                    Log.Info(String.Format("Server answer {0} {1}", result.ResultCodes, result.Description));
                    CheckSignature(secondResult);
                }
                else
                {
                    Log.Warn("Result is null");
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
            catch
            {
                Log.Error("Unknown error");
            }
        }


        private void SendPaymentThread()
        {
            var listToDelete = new Queue<long>();

            while (_Running && _Init && _AuthTerminal)
            {
                try
                {
                    var list = _Db.GetTransactions();

                    foreach (var row in list)
                    {
                        var values = _Db.GetPaymentValues(row.Id);
                        var banknotes = _Db.GetPaymentBanknotes(row.Id);

                        var now = DateTime.Now;
                        var request = new PaymentInfoByProducts
                            {
                                TerminalDate = now,
                                SystemTime = now,
                                TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                                TransactionId = row.TransactionId,
                                ProductId = (int)row.ProductId,
                                Currency = row.Currency,
                                CurrencyRate = (float)row.CurrencyRate,
                                Amount = (int)row.Amount,
                                Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey)
                            };

                        var valuesList = new List<String>();
                        var banknotesList = new List<int>();

                        foreach (var value in values)
                        {
                            valuesList.Add(value.Value);
                        }

                        foreach (var banknote in banknotes)
                        {
                            banknotesList.Add((int)banknote.Amount);
                        }

                        request.Banknotes = banknotesList.ToArray();
                        request.Values = valuesList.ToArray();

                        var response = _Server.Payment(request);

                        if (response != null && response.ResultCodes == ResultCodes.Ok)
                        {
                            listToDelete.Enqueue(row.Id);
                        }
                        else if (response != null)
                        {
                            Log.Warn(String.Format("Server answer: {0}, {1}", response.ResultCodes, response.Description));
                        }
                        else
                        {
                            Log.Warn("Response is null");
                        }
                    }
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                }
                catch
                {
                    Log.Error("Non cachable exception");
                }

                try
                {
                    if (listToDelete.Count > 0)
                    {
                        foreach (var item in listToDelete)
                        {
                            _Db.DeleteTransaction(item);
                        }
                    }
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                }
                catch
                {
                    Log.Error("Non cachable exception");
                }
                Thread.Sleep(SEND_PAYMENT_TIMEOUT);
            }
        }

        private void CheckSignature(StandardResult request)
        {
            if (!Utilities.CheckSignature(Settings.Default.TerminalCode, request.SystemTime, request.Sign, _LocalKeys))
            {
                throw new Exception("Invalid signature");
            }
        }
    }
}
