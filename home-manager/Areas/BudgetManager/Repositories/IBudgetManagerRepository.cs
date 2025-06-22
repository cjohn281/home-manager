using home_manager.Areas.BudgetManager.Models;
using home_manager.Areas.BudgetManager.ViewModels;

namespace home_manager.Areas.BudgetManager.Repositories
{
    public interface IBudgetManagerRepository
    {
        Task<(int, int)> GetLatestAvailableLedger();
        Task<IEnumerable<int>> GetAvailableLedgerMonths();
        Task<IEnumerable<int>> GetAvailableLedgerYears();
        Task<IEnumerable<LedgerItem_VModel>> GetLedgerItemsByMonth(int month, int year);
        Task<(decimal, decimal)> GetEndingBalances(int month, int year);
        Task<bool> UpdateLedgerItem(int id, decimal amount, bool isPaid, DateTime date);
        Task<IEnumerable<IncomeDetail>> GetIncomeDetails();
        Task<IEnumerable<RecurringSavingsTransferDetail>> GetRecurringSavingsTransferDetails();
        Task<DateTime> GetLatestPayDate(int prnId);
        Task<DateTime> GetLatestRecurringSavingsDate(int rsdId);
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
        Task<IEnumerable<Category>> GetIncidentalCategories();
        Task<IEnumerable<Category>> GetCategoriesByTransactionId(int transactionId);
        Task<bool> UpdateIncidentalItem(IncidentalItem item, int month, int year);
        Task<bool> DeleteIncidentalItem(int itemId);


    }
}
