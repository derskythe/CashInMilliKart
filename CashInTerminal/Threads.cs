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
        private const int PING_TIMEOUT = 60 * 1000;
        private const int SEND_PAYMENT_TIMEOUT = 1000;

        private void PingThread()
        {
            while (_Running && _Init && _AuthTerminal)
            {
                try
                {
                    var request = new PingRequest
                        {
                            CashCodeStatus = (int) _CcnetDevice.DeviceState.StateCode,
                            SystemTime = DateTime.Now,
                            TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                            TerminalStatus = !_EncashmentMode ? (int) TerminalCodes.Ok : (int)TerminalCodes.Encashment
                        };
                    var result = _Server.Ping(request);

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
                                    ChangePannel(pnlOutOfOrder);
                                    break;

                                case TerminalCommands.TestMode:
                                    Log.Warn("Received command " + TerminalCommands.TestMode.ToString());
                                    ChangePannel(pnlTestMode);
                                    break;

                                case TerminalCommands.Idle:
                                case TerminalCommands.NormalMode:
                                    if (_SelectedPanel == "pnlOutOfOrder" || _SelectedPanel == "pnlTestMode")
                                    {
                                        ChangePannel(pnlProducts);
                                    }
                                    break;
                            }

                            var cmd = new StandardRequest
                                {
                                    TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                                    SystemTime = DateTime.Now
                                };
                            var secondResult = _Server.CommandReceived(cmd);

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

                        var request = new PaymentInfoByProducts();
                        request.TerminalDate = DateTime.Now;
                        request.TerminalId = Convert.ToInt32(Settings.Default.TerminalCode);
                        request.TransactionId = row.TransactionId;
                        request.ProductId = (int)row.ProductId;
                        request.Currency = row.Currency;
                        request.CurrencyRate = (float)row.CurrencyRate;
                        request.Amount = (int)row.Amount;

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
                            Log.Error(String.Format("Server answer: {0}, {1}", response.ResultCodes, response.Description));
                        }
                        else
                        {
                            Log.Error("Response is null");
                        }
                    }
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
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
                Thread.Sleep(SEND_PAYMENT_TIMEOUT);
            }
        }
    }
}
