using home_manager.Areas.BudgetManager.Models;
using home_manager.Areas.BudgetManager.ViewModels;

namespace home_manager.Areas.BudgetManager.Repositories
{
    public interface IBudgetManagerRepository
    {
        Task<bool> LedgerExists(int month, int year);
        Task<(int, int)> GetLatestAvailableLedger();
        Task<IEnumerable<int>> GetAvailableLedgerMonths();
        Task<IEnumerable<int>> GetAvailableLedgerYears();
        Task<IEnumerable<LedgerItem_VModel>> GetLedgerItemsByMonth(int month, int year);
        Task<LedgerItem_VModel> GetLedgerItemById(int id);
        Task<(decimal, decimal)> GetEndingBalances(int month, int year);
        Task<(decimal, decimal)> GetBalanceDetails(int month, int year);
        Task<decimal> GetBalanceBeforeNextPay(int month, int year);
        Task<bool> UpdateLedgerItem(int id, decimal amount, bool isPaid, DateTime date);
        Task<IEnumerable<RecurringSavingsTransferDetail>> GetRecurringSavingsTransferDetails();
        Task<bool> InsertNewLedger(int month, int year);
        Task<bool> UpdateIncomeLedgerItem(IncomeLedgerItem item);
        Task<bool> UpdateSavingsLedgerItem(SavingsLedgerItem item);


        Task<IEnumerable<Category>> GetRecurringCategoryFilterItems();
        Task<IEnumerable<RecurringItem>> GetRecurringItemsByCategoryId(int categoryId);
        Task<RecurringItem> GetRecurringItemById(int recurringItemId);
        Task<bool> UpdateRecurringItem(RecurringItem item);
        Task<bool> DeleteRecurringItem(int itemId);

        
        Task<IEnumerable<IncidentalItem>> GetIncidentalItems(int month, int year);
        Task<IEnumerable<TransactionType>> GetIncidentalTransactionTypes();
        Task<IncidentalItem> GetIncidentalItemById(int Id);
        Task<IEnumerable<Category>> GetIncidentalCategories();
        Task<IEnumerable<Category>> GetCategoriesByTransactionId(int transactionId);
        Task<bool> UpdateIncidentalItem(IncidentalItem item, int month, int year);
        Task<bool> DeleteIncidentalItem(int itemId);


        Task<IEnumerable<IncomeLedgerItem>> GetIncomeLedgerItems(int month, int year);
        Task<IncomeLedgerItem> GetIncomeLedgerItemById(int id);
        Task<IEnumerable<SavingsLedgerItem>> GetSavingsLedgerItems(int month, int year);
        Task<SavingsLedgerItem> GetSavingsLedgerItemById(int id);
        Task<bool> DeleteSavingsLedgerItem(int itemId);

        Task<IEnumerable<Person>> GetPersonList();
        Task<IEnumerable<IncomeDetail>> GetIncomeDetails();
        Task<IEnumerable<LookupValue_VModel>> GetLookupValuesByType(int type_id);
    }
}
