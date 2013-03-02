using System;
using System.ServiceModel;
using Containers;
using Containers.Admin;
using Containers.Enums;

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
        ListUsersResult ListUsers(String sid, UsersColumns sortColumn, SortType sortType);

        [OperationContract]
        ListRolesResult ListRoles(String sid);

        [OperationContract]
        ListTerminalsResult ListTerminals(string sid, TerminalColumns sortColumn, SortType sortType);

        [OperationContract]
        ListEncashmentResult ListEncashment(string sid, EncashmentColumns sortColumn, SortType sortType);

        [OperationContract]
        ListProductsResult ListProducts(String sid);

        [OperationContract]
        ListProductHistoryResult ListProductHistory(string sid, ProductHistoryColumns sortColumn, SortType sortType);

        [OperationContract]
        ListProductHistoryResult ListProductHistoryByDate(string sid, DateTime from, DateTime to, ProductHistoryColumns sortColumn, SortType sortType);

        [OperationContract]
        ListProductHistoryResult ListProductHistoryByTransactionId(string sid, string transactionId, ProductHistoryColumns sortColumn, SortType sortType);

        [OperationContract]
        User GetUser(String sid, String username);

        [OperationContract]
        User GetUserById(String sid, int id);

        [OperationContract]
        TerminalInfoResult GetTerminal(String sid, int terminalId);

        [OperationContract]
        SaveTerminalResult SaveTerminal(String sid, Terminal terminal);

        [OperationContract]
        ListCurrenciesResult ListCurrencies(String sid, CurrencyColumns sortColumn, SortType sortType);

        [OperationContract]
        StandardResult UpdateProfile(String sid, string newPassword);

        [OperationContract]
        EncashmentResult GetEncashment(String sid, int id);

        [OperationContract]
        CurrencyResult GetCurrency(String sid, string id);
    }
}