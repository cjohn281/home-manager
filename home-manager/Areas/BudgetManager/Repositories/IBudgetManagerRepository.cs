using home_manager.Areas.BudgetManager.Models;

namespace home_manager.Areas.BudgetManager.Repositories
{
    public interface IBudgetManagerRepository
    {
        Task<IEnumerable<RecurringItem>> GetRecurringItemsByCategoryIdAsync(int categoryId);
        Task<RecurringItem> GetRecurringItemByIdAsync(int recurringItemId);
        Task<IEnumerable<Category>> GetRecurringCategoryFilterItemsAsync();
        Task<IEnumerable<int>> GetAvailableLedgerMonthsAsync();
        Task<IEnumerable<int>> GetAvailableLedgerYearsAsync();
        Task<IEnumerable<IncidentalItem>> GetIncidentalItemsAsync(int month, int year);
        Task<IEnumerable<TransactionType>> GetIncidentalTransactionTypesAsync();
        Task<IEnumerable<Category>> GetIncidentalCategoriesAsync();
    }
}
