using Dapper;
using home_manager.Areas.BudgetManager.Models;
using home_manager.Data;
using Npgsql;

namespace home_manager.Areas.BudgetManager.Repositories
{
    public class BudgetManagerRepository : IBudgetManagerRepository
    {

        private readonly DbConnectionService _dbConnection;


        public BudgetManagerRepository(DbConnectionService dbConnection)
        {
            _dbConnection = dbConnection;
        }


        public async Task<IEnumerable<int>> GetAvailableLedgerMonthsAsync()
        {
            using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
            await connection.OpenAsync();
            var months = (await connection.QueryAsync<int>(
                "SELECT * FROM fnc_get_available_ledger_months()", new { }
            ))?.ToList();

            if (months == null || months.Count == 0)
            {
                return new List<int> { 0 };
            }

            return months;
        }


        public async Task<IEnumerable<int>> GetAvailableLedgerYearsAsync()
        {
            using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
            await connection.OpenAsync();
            var years = (await connection.QueryAsync<int>(
                "SELECT * FROM fnc_get_available_ledger_years()", new { }
            ))?.ToList();

            if (years == null || years.Count == 0)
            {
                return new List<int> { 0 };
            }

            return years;
        }


        public async Task<IEnumerable<Category>> GetIncidentalCategoriesAsync()
        {
            using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
            await connection.OpenAsync();
            var categories = (await connection.QueryAsync<Category>(
                "SELECT * FROM fnc_get_incidental_categories()", new { }
            ))?.ToList();

            if (categories == null || categories.Count == 0)
            {
                return new List<Category> { new() };
            }

            return categories;
        }


        public async Task<IEnumerable<IncidentalItem>> GetIncidentalItemsAsync(int month, int year)
        {
            using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
            await connection.OpenAsync();
            var items = (await connection.QueryAsync<IncidentalItem>(
                "SELECT * FROM fnc_get_incidental_items_by_month(@month, @year)", new { month, year }
            ))?.ToList();

            if (items == null || items.Count == 0)
            {
                return new List<IncidentalItem> { new IncidentalItem() };
            }

            return items;
        }


        public async Task<IEnumerable<TransactionType>> GetIncidentalTransactionTypesAsync()
        {
            using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
            var transactions = (await connection.QueryAsync<TransactionType>(
                "SELECT * FROM fnc_get_incidental_transaction_types()", new { }
            )).ToList();

            if (transactions == null || transactions.Count == 0)
            {
                return new List<TransactionType> { new TransactionType() };
            }

            return transactions;
        }


        public async Task<IEnumerable<Category>> GetRecurringCategoryFilterItemsAsync()
        {
            using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
            await connection.OpenAsync();
            var items = (await connection.QueryAsync<Category>(
                "SELECT * FROM fnc_get_recurring_category_filter_items();"
            )).ToList();

            if (items == null || items.Count == 0)
            {
                return new List<Category> { new Category() };
            }

            return items;
        }


        public async Task<RecurringItem> GetRecurringItemByIdAsync(int recurringItemId)
        {
            using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
            await connection.OpenAsync();
            var item = await connection.QueryFirstOrDefaultAsync<RecurringItem>(
                "SELECT * FROM fnc_get_recurring_item_by_id(@itemId)", new { recurringItemId }
            );

            if (item == null)
            {
                return new RecurringItem();
            }

            return item;
        }


        public async Task<IEnumerable<RecurringItem>> GetRecurringItemsByCategoryIdAsync(int categoryId)
        {
            using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
            await connection.OpenAsync();
            var items = (await connection.QueryAsync<RecurringItem>(
                "SELECT * FROM fnc_get_recurring_items_by_category(@categoryId)", new { categoryId }
            ))?.ToList();

            if (items == null || items.Count == 0)
            {
                return new List<RecurringItem> { new RecurringItem() };
            }

            return items;
        }
    }
}
