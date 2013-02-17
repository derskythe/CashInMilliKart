using System;
using System.ServiceModel;
using Containers;
using Containers.Admin;

namespace CashInCore
{
    [ServiceContract(Namespace = "http://cashin/CashInAdminService",
        Name = "CashInAdminService"), XmlSerializerFormat]
    public interface ICashInAdminServer
    {
        [OperationContract]
        LoginResult Login(String username, String password);

        [OperationContract]
        SessionResult CheckSession(String sid);

        [OperationContract]
        StandardResult SaveUser(String sid, User userInfo);

        [OperationContract]
        StandardResult DeleteUser(String sid, int id);

        [OperationContract]
        StandardResult SetStatusCode(String sid, int terminalId, int statusCode);

        [OperationContract]
        StandardResult SaveUserRole(String sid, int userId, int roleId);

        [OperationContract]
        StandardResult SaveCurrency(String sid, Currency currency);

        [OperationContract]
        StandardResult SaveCurrencyRate(String sid, String currency, decimal rate);

        [OperationContract]
        ListUsersResult ListUsers(String sid);

        [OperationContract]
        ListRolesResult ListRoles(String sid);

        [OperationContract]
        ListTerminalsResult ListTerminals(String sid);

        [OperationContract]
        ListEncashmentResult ListEncashment(String sid);

        [OperationContract]
        ListProductsResult ListProducts(String sid);

        [OperationContract]
        ListProductHistoryResult ListProductHistory(String sid);

        [OperationContract]
        ListProductHistoryResult ListProductHistoryByDate(String sid, DateTime from, DateTime to);

        [OperationContract]
        ListProductHistoryResult ListProductHistoryByTransactionId(String sid, String transactionId);

        [OperationContract]
        User GetUser(String sid, String username);

        [OperationContract]
        User GetUserById(String sid, int id);

        [OperationContract]
        TerminalInfoResult GetTerminal(String sid, int terminalId);

        [OperationContract]
        StandardResult SaveTerminal(String sid, Terminal terminal);
    }
}