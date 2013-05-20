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
        ListTerminalTypeResult ListTerminalTypes(String sid);

        [OperationContract]
        ListTerminalsResult ListTerminalsByType(string sid, int type, TerminalColumns sortColumn, SortType sortType,
                                                int rowNum, int perPage);

        [OperationContract]
        StandardResult SaveCurrency(String sid, Currency currency);

        [OperationContract]
        StandardResult SaveCurrencyRate(String sid, String currency, decimal rate);

        [OperationContract]
        ListUsersResult ListUsers(String sid, UsersColumns sortColumn, SortType sortType);

        [OperationContract]
        ListRolesResult ListRoles(String sid);

        [OperationContract]
        ListTerminalsResult ListTerminals(string sid, TerminalColumns sortColumn, SortType sortType, int rowNum,
                                          int perPage);

        [OperationContract]
        ListEncashmentResult ListEncashment(string sid, EncashmentColumns sortColumn, SortType sortType, int rowNum,
                                            int perPage);

        [OperationContract]
        ListProductsResult ListProducts(String sid);

        [OperationContract]
        ListProductHistoryResult ListProductHistory(string sid, ProductHistoryColumns sortColumn, SortType sortType,
                                                    int rowNum, int perPage);

        [OperationContract]
        ListProductHistoryResult ListProductHistoryByDate(string sid, DateTime from, DateTime to,
                                                          ProductHistoryColumns sortColumn, SortType sortType,
                                                          int rowNum, int perPage);

        [OperationContract]
        ListProductHistoryResult ListProductHistoryByTransactionId(string sid, string transactionId,
                                                                   ProductHistoryColumns sortColumn, SortType sortType);

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

        [OperationContract]
        ListBanknotesResult GetBanknotesByTerminal(String sid, int terminalId);

        [OperationContract]
        ListBanknotesResult GetBanknotesByEncashment(String sid, int encashmentId);

        [OperationContract]
        ListBanknotesResult GetBanknotesByHistory(String sid, int historyId);

        [OperationContract]
        ListCheckFieldTypeResult ListCheckFieldTypes(String sid);

        [OperationContract]
        ListCheckTypeResult ListCheckTypes(String sid);

        [OperationContract]
        ListCheckTemplateResult ListCheckTemplates(String sid);

        [OperationContract]
        ListCheckTemplateResult GetCheckTemplate(String sid, int id);

        [OperationContract]
        StandardResult SaveCheckTemplate(String sid, CheckTemplate item);

        [OperationContract]
        StandardResult DeleteCheckTemplate(String sid, int id);

        [OperationContract]
        StandardResult ActivateCheckTemplate(String sid, int id, bool activationStatus);

        [OperationContract]
        ListBranchesResult ListBranches(String sid, BranchColumns column, SortType sortType);

        [OperationContract]
        ListBranchesResult GetBranch(String sid, int id);

        [OperationContract]
        StandardResult SaveBranch(String sid, Branch branch);

        [OperationContract]
        CheckField GetCheckField(String sid, int id);

        [OperationContract]
        ListCheckTemplateResult GetCheckTemplates(int id);

        [OperationContract]
        ListEncashmentResult ListEncashmentByTerminal(string sid, int terminalId, EncashmentColumns sortColumn,
                                                      SortType sortType, int rowNum, int perPage);

        [OperationContract]
        ListTerminalsResult ListTerminalsByTerminalStatus(string sid, int statusId, TerminalColumns sortColumn,
                                                          SortType sortType, int rowNum, int perPage);
        [OperationContract]
        ListTerminalsResult ListTerminalsByBranchId(string sid, int id, TerminalColumns sortColumn, SortType sortType,
                                                    int rowNum, int perPage);

        [OperationContract]
        ListEncashmentResult ListEncashmentByBranchId(string sid, int branchId, EncashmentColumns sortColumn,
                                                      SortType sortType, int rowNum, int perPage);
        [OperationContract]
        ListEncashmentResult ListEncashmentByDate(string sid, DateTime from, DateTime to, EncashmentColumns sortColumn,
                                                  SortType sortType, int rowNum, int perPage);
        [OperationContract]
        ListTerminalStatusResult ListTerminalStatusCode(String sid);

        [OperationContract]
        ListEncashmentResult ListEncashmentByAllParams(string sid, int terminalId, int branchId, DateTime from,
                                                       DateTime to, EncashmentColumns sortColumn, SortType sortType,
                                                       int rowNum, int perPage);

        [OperationContract]
        ListProductHistoryResult ListProductHistoryByTerminalId(string sid, DateTime from, DateTime to, int terminalId,
                                                                ProductHistoryColumns sortColumn, SortType sortType,
                                                                int rowNum, int perPage);
        
        [OperationContract]
        ListProductHistoryResult ListProductHistoryByEncashmentId(string sid, DateTime from, DateTime to,
                                                                  int encashmentId, ProductHistoryColumns sortColumn,
                                                                  SortType sortType, int rowNum, int perPage);
                
        [OperationContract]
        ListProductHistoryResult ListProductHistoryByProductId(string sid, DateTime from, DateTime to, int productId,
                                                               ProductHistoryColumns sortColumn, SortType sortType,
                                                               int rowNum, int perPage);        
        [OperationContract]
        ListBanknoteSummary ListBanknoteSummaryByTerminalId(String sid, int terminalId);

        [OperationContract]
        ListBanknoteSummary ListBanknoteSummaryByHistoryId(String sid, int historyId);

        [OperationContract]
        ListBanknoteSummary ListBanknoteSummaryByEncashmentId(String sid, int encashmentId);

        [OperationContract]
        StandardResult SaveUserBranch(String sid, int userId, int[] branchId);

        [OperationContract]
        StandardResult SaveUserRole(String sid, int userId, int[] roles);

        [OperationContract]
        ListUsersResult ListUsersByUsername(String sid, String username, UsersColumns sortColumn, SortType sortType);

        [OperationContract]
        SaveTerminalResult DeleteTerminal(String sid, int terminalId);
    }
}