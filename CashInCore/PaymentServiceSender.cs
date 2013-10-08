using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using CashInCore.PaymentService;
using Containers;
using Containers.Enums;
using Db;
using NLog;

namespace CashInCore
{
    public class PaymentServiceSender : IDisposable
    {
        private readonly String _MultiPaymentUsername;
        private readonly String _MultiPaymentPassword;
        private bool _Disposed;
        private bool _Started;
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local
        private Thread _SendThread;
        private Thread _GetStatusThread;

        public PaymentServiceSender(string multiPaymentUsername, string multiPaymentPassword)
        {
            _MultiPaymentUsername = multiPaymentUsername;
            _MultiPaymentPassword = multiPaymentPassword;
        }

        public void Start()
        {
            _Started = true;
            try
            {
                _SendThread = new Thread(SendThread);
                _SendThread.Start();

                _GetStatusThread = new Thread(GetStatusThread);
                _GetStatusThread.Start();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void GetStatusThread()
        {
            while (_Started)
            {
                try
                {
                    var list = OracleDb.Instance.ListOtherPaymentsRequest((int)OtherPaymentsRequestStatus.Requested).ToList();

                    if (list.Count > 0)
                    {
                        foreach (ds.V_ACTIVE_OTHER_PAYMENTS_EXTRow row in list)
                        {
                            if (row.IsTRANSACTION_IDNull())
                            {
                                Log.Warn(String.Format("TransactionID is null. {0}", row.ID));
                                OracleDb.Instance.SaveStatusOtherPaymentsRequest(row.ID,
                                                                                 OtherPaymentsRequestStatus.Done);
                                continue;
                            }

                            var client = new PaymentServiceClient();
                            var transactionResult = client.GetTransactionInfo(_MultiPaymentUsername,
                                                                              _MultiPaymentPassword,
                                                                              row.TRANSACTION_ID);
                            if (transactionResult.code == ErrorCodes.Ok)
                            {
                                var historyRow = OracleDb.Instance.GetProductHistory(row.PRODUCTS_HISTORY_ID);

                                if (historyRow == null)
                                {
                                    Log.Error(String.Format("History row not found. {0}", row.PRODUCTS_HISTORY_ID));
                                    OracleDb.Instance.SaveStatusOtherPaymentsRequest(row.ID,
                                                                                     OtherPaymentsRequestStatus.Done);
                                    continue;
                                }

                                string bills = String.Join(";",
                                                           OracleDb.Instance.ListBanknotesByHistoryId(historyRow.Id,
                                                                                                      true));

                                OracleDb.Instance.CommitPayment(historyRow.CreditNumber,
                                                                historyRow.Amount,
                                                                bills,
                                                                historyRow.TerminalId,
                                                                historyRow.PaymentType,
                                                                historyRow.TerminalDate,
                                                                historyRow.CurrencyId,
                                                                historyRow.TransactionId);

                                OracleDb.Instance.SaveStatusOtherPaymentsRequest(row.ID,
                                                                                 OtherPaymentsRequestStatus.Done);
                            }
                            else
                            {
                                OracleDb.Instance.SaveStatusOtherPaymentsRequest(row.ID,
                                                                                 OtherPaymentsRequestStatus.Requested);
                            }
                        }
                    }
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                }

                Thread.Sleep(5 * 1000);
            }
        }

        private void SendThread()
        {
            while (_Started)
            {
                try
                {
                    var list = OracleDb.Instance.ListOtherPaymentsRequest((int)OtherPaymentsRequestStatus.None).ToList();

                    if (list.Count > 0)
                    {
                        var ser = new DataContractJsonSerializer(typeof(TerminalPaymentInfo));

                        foreach (ds.V_ACTIVE_OTHER_PAYMENTS_EXTRow row in list)
                        {
                            if (String.IsNullOrEmpty(row.VALUE))
                            {
                                continue;
                            }

                            TerminalPaymentInfo otherObject;
                            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(row.VALUE)))
                            {
                                otherObject = (TerminalPaymentInfo)ser.ReadObject(memoryStream);
                            }

                            if (otherObject == null)
                            {
                                continue;
                            }

                            var client = new PaymentServiceClient();
                            var servicesResult = client.GetService(_MultiPaymentUsername, _MultiPaymentPassword,
                                                                   otherObject.PaymentServiceId);

                            if (servicesResult.code != ErrorCodes.Ok)
                            {
                                throw new Exception(servicesResult.description);
                            }

                            if (servicesResult.services == null || servicesResult.services.Length == 0)
                            {
                                throw new Exception("Service is null");
                            }
                            var service = servicesResult.services[0];
                            var fields = new List<HistoryField>();
                            int i = 0;
                            foreach (var field in service.service_fields)
                            {
                                var history = new HistoryField
                                    {
                                        name = field.name,
                                        value = !String.IsNullOrEmpty(field.default_value) ? field.default_value : otherObject.Values[i]
                                    };

                                fields.Add(history);
                                i++;
                            }

                            var info = new PurePaymentInfo
                                {
                                    amount = otherObject.Amount * 100,
                                    currency = "AZN",
                                    payment_id = otherObject.TransactionId,
                                    service_id = service.id,
                                    service_name = service.name,
                                    fields = fields.ToArray()
                                };

                            Log.Debug(info);

                            var serviceResult = client.InsertPaymentPure(_MultiPaymentUsername, _MultiPaymentPassword, "CashIn", info);
                            if (serviceResult.code != ErrorCodes.Ok)
                            {
                                Log.Error(serviceResult.description);
                            }
                            else
                            {
                                OracleDb.Instance.SaveStatusOtherPaymentsRequest(row.ID,
                                                                                 OtherPaymentsRequestStatus.Requested);
                            }
                        }
                    }
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                }

                Thread.Sleep(10 * 1000);
            }
        }

        public void Stop()
        {
            _Started = false;
        }

        public void Dispose()
        {
            _Started = false;
            _Disposed = true;
        }
    }
}
