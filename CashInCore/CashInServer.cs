using System;
using System.Text;
using Containers;
using Db;
using NLog;
using Org.BouncyCastle.Utilities.Encoders;

namespace CashInCore
{
    public partial class CashInServer
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local


        public AuthResult InitTerminal(int terminalId, string authKey, string publicKey)
        {
            Log.Info("InitTerminal");
            var result = new AuthResult();

            try
            {
                if (String.IsNullOrEmpty(publicKey))
                {
                    result.Code = ResultCodes.InvalidKey;
                    throw new Exception("Codes not equal");
                }

                if (String.IsNullOrEmpty(authKey))
                {
                    result.Code = ResultCodes.InvalidParameters;
                    throw new Exception("Auth code is NULL!");
                }

                var terminal = OracleDb.Instance.GetTerminal(terminalId);

                if (terminal == null)
                {
                    result.Code = ResultCodes.InvalidTerminal;
                    throw new Exception("Terminal not found in DB");
                }

                if (terminal.TmpKey != Encoding.ASCII.GetBytes(authKey))
                {
                    result.Code = ResultCodes.InvalidTerminal;
                    throw new Exception("Codes not equal");
                }

                OracleDb.Instance.UpdateTerminalStatus((int)TerminalCodes.Ok, 0);
                OracleDb.Instance.UpdateTerminalKey(terminalId, UrlBase64.Decode(publicKey));

                result.Code = ResultCodes.Ok;

                Log.Info("Init terminal " + terminal + " Terminal info:\n" + terminal);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        public StandardResult Ping(int terminalId, string signature)
        {
            
        }
    }
}
