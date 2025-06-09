using Dapper;
using home_manager.Areas.BudgetManager.Models;
using home_manager.Areas.BudgetManager.ViewModels;
using home_manager.Data;
using Npgsql;
using System.Data;
using static home_manager.Helpers.DropdownHelper;

namespace home_manager.Areas.BudgetManager.Repositories
{
    public class BudgetManagerRepository : IBudgetManagerRepository
    {

        private readonly DbConnectionService _dbConnection;


        public BudgetManagerRepository(DbConnectionService dbConnection)
        {
            _dbConnection = dbConnection;
        }


        public async Task<(int, int)> GetLatestAvailableLedger()
        {
            try
            {
                using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
                await connection.OpenAsync();

                string sql = "SELECT * FROM fnc_get_latest_available_ledger()";

                await using var command = new NpgsqlCommand(sql, connection);
                await using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    int month = reader.GetInt32(0);
                    int year = reader.GetInt32(1);

                    return (month, year);
                }
                else
                {
                    return (0, 0);
                }
            }
            catch (PostgresException pgEx)
            {
                System.Diagnostics.Debug.WriteLine($"PostgreSQL Error: {pgEx.MessageText}");
                System.Diagnostics.Debug.WriteLine($"Detail: {pgEx.Detail}");
                System.Diagnostics.Debug.WriteLine($"Hint: {pgEx.Hint}");
                System.Diagnostics.Debug.WriteLine($"Position: {pgEx.Position}");
                System.Diagnostics.Debug.WriteLine($"SqlState: {pgEx.SqlState}");
                return (0, 0);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in GetLatestAvailableLedger: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return (0, 0);
            }

        }


        public async Task<IEnumerable<int>> GetAvailableLedgerMonths()
        {
            try
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
            catch (PostgresException pgEx)
            {
                System.Diagnostics.Debug.WriteLine($"PostgreSQL Error: {pgEx.MessageText}");
                System.Diagnostics.Debug.WriteLine($"Detail: {pgEx.Detail}");
                System.Diagnostics.Debug.WriteLine($"Hint: {pgEx.Hint}");
                System.Diagnostics.Debug.WriteLine($"Position: {pgEx.Position}");
                System.Diagnostics.Debug.WriteLine($"SqlState: {pgEx.SqlState}");
                return new List<int> { 0 };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in GetAvailableLedgerMonths: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new List<int> { 0 };
            }

        }


        public async Task<IEnumerable<LedgerItem_VModel>> GetLedgerItemsByMonth(int month, int year)
        {
            try
            {
                using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
                await connection.OpenAsync();

                var parameters = new
                {
                    month = month,
                    year = year
                };

                var ledgerItems = (await connection.QueryAsync<LedgerItem_VModel>(
                    "SELECT * FROM fnc_get_ledger_items_by_month(@month, @year)", parameters
                ))?.ToList();

                if (ledgerItems == null || ledgerItems.Count == 0)
                {
                    return new List<LedgerItem_VModel> { new LedgerItem_VModel() };
                }

                return ledgerItems;
            }
            catch (PostgresException pgEx)
            {
                System.Diagnostics.Debug.WriteLine($"PostgreSQL Error: {pgEx.MessageText}");
                System.Diagnostics.Debug.WriteLine($"Detail: {pgEx.Detail}");
                System.Diagnostics.Debug.WriteLine($"Hint: {pgEx.Hint}");
                System.Diagnostics.Debug.WriteLine($"Position: {pgEx.Position}");
                System.Diagnostics.Debug.WriteLine($"SqlState: {pgEx.SqlState}");
                return new List<LedgerItem_VModel> { new LedgerItem_VModel() };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in GetLedgerItems: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new List<LedgerItem_VModel> { new LedgerItem_VModel() };
            }
        }


        public async Task<(decimal, decimal)> GetStartingBalances(int month, int year)
        {
            try
            {
                using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
                await connection.OpenAsync();

                string sql = "SELECT * FROM fnc_get_starting_balances_by_month(@month, @year)";

                await using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("month", NpgsqlTypes.NpgsqlDbType.Integer, month);
                command.Parameters.AddWithValue("year", NpgsqlTypes.NpgsqlDbType.Integer, year);

                await using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    decimal checkingStartingBalance = reader.GetDecimal(0);
                    decimal savingsStartingBalance = reader.GetDecimal(1);

                    return (checkingStartingBalance, savingsStartingBalance);
                }
                else
                {
                    return (0, 0);
                }
            }
            catch (PostgresException pgEx)
            {
                System.Diagnostics.Debug.WriteLine($"PostgreSQL Error: {pgEx.MessageText}");
                System.Diagnostics.Debug.WriteLine($"Detail: {pgEx.Detail}");
                System.Diagnostics.Debug.WriteLine($"Hint: {pgEx.Hint}");
                System.Diagnostics.Debug.WriteLine($"Position: {pgEx.Position}");
                System.Diagnostics.Debug.WriteLine($"SqlState: {pgEx.SqlState}");
                return (0, 0);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in GetStartingBalances: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return (0, 0);
            }

        }


        public async Task<bool> UpdateLedgerItem(int id, decimal amount, bool isPaid, DateTime date)
        {
            try
            {
                using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("id", id, DbType.Int32);
                parameters.Add("amount", amount, DbType.Decimal);
                parameters.Add("isPaid", isPaid, DbType.Boolean);
                parameters.Add("date", date, DbType.Date);
                

                var result = await connection.ExecuteAsync(
                    @"CALL spw_update_ledger_item(
                    @id, @amount, @isPaid, @date
                    )",
                    parameters
                );

                return true;
            }
            catch (PostgresException pgEx)
            {
                System.Diagnostics.Debug.WriteLine($"PostgreSQL Error: {pgEx.MessageText}");
                System.Diagnostics.Debug.WriteLine($"Detail: {pgEx.Detail}");
                System.Diagnostics.Debug.WriteLine($"Hint: {pgEx.Hint}");
                System.Diagnostics.Debug.WriteLine($"Position: {pgEx.Position}");
                System.Diagnostics.Debug.WriteLine($"SqlState: {pgEx.SqlState}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in UpdateLedgerItem: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return false;
            }
        }


        public async Task<IEnumerable<int>> GetAvailableLedgerYears()
        {
            try
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
            catch (PostgresException pgEx)
            {
                System.Diagnostics.Debug.WriteLine($"PostgreSQL Error: {pgEx.MessageText}");
                System.Diagnostics.Debug.WriteLine($"Detail: {pgEx.Detail}");
                System.Diagnostics.Debug.WriteLine($"Hint: {pgEx.Hint}");
                System.Diagnostics.Debug.WriteLine($"Position: {pgEx.Position}");
                System.Diagnostics.Debug.WriteLine($"SqlState: {pgEx.SqlState}");
                return new List<int> { 0 };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in GetAvailableLedgerYears: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new List<int> { 0 };
            }
        }


        public async Task<IEnumerable<Category>> GetRecurringCategoryFilterItems()
        {
            try
            {
                using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
                await connection.OpenAsync();

                var items = (await connection.QueryAsync<Category>(
                    "SELECT * FROM fnc_get_recurring_category_filter_items()"
                )).ToList();

                if (items == null || items.Count == 0)
                {
                    return new List<Category> { new Category() };
                }

                return items;
            }
            catch (PostgresException pgEx)
            {
                System.Diagnostics.Debug.WriteLine($"PostgreSQL Error: {pgEx.MessageText}");
                System.Diagnostics.Debug.WriteLine($"Detail: {pgEx.Detail}");
                System.Diagnostics.Debug.WriteLine($"Hint: {pgEx.Hint}");
                System.Diagnostics.Debug.WriteLine($"Position: {pgEx.Position}");
                System.Diagnostics.Debug.WriteLine($"SqlState: {pgEx.SqlState}");
                return new List<Category> { new Category() };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in GetRecurringCategoryFilterItems: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new List<Category> { new Category() };
            }
        }


        public async Task<IEnumerable<RecurringItem>> GetRecurringItemsByCategoryId(int categoryId)
        {
            try
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
            catch (PostgresException pgEx)
            {
                System.Diagnostics.Debug.WriteLine($"PostgreSQL Error: {pgEx.MessageText}");
                System.Diagnostics.Debug.WriteLine($"Detail: {pgEx.Detail}");
                System.Diagnostics.Debug.WriteLine($"Hint: {pgEx.Hint}");
                System.Diagnostics.Debug.WriteLine($"Position: {pgEx.Position}");
                System.Diagnostics.Debug.WriteLine($"SqlState: {pgEx.SqlState}");
                return new List<RecurringItem> { new RecurringItem() };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in GetRecurringItemsByCategoryId: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new List<RecurringItem> { new RecurringItem() };
            }
        }


        public async Task<RecurringItem> GetRecurringItemById(int recurringItemId)
        {
            try
            {
                using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
                await connection.OpenAsync();

                var item = await connection.QueryFirstOrDefaultAsync<RecurringItem>(
                    "SELECT * FROM fnc_get_recurring_item_by_id(@recurringItemId)", new { recurringItemId }
                );

                if (item == null)
                {
                    return new RecurringItem();
                }

                return item;
            }
            catch (PostgresException pgEx)
            {
                System.Diagnostics.Debug.WriteLine($"PostgreSQL Error: {pgEx.MessageText}");
                System.Diagnostics.Debug.WriteLine($"Detail: {pgEx.Detail}");
                System.Diagnostics.Debug.WriteLine($"Hint: {pgEx.Hint}");
                System.Diagnostics.Debug.WriteLine($"Position: {pgEx.Position}");
                System.Diagnostics.Debug.WriteLine($"SqlState: {pgEx.SqlState}");
                return new RecurringItem();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in GetRecurringItemById: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new RecurringItem();
            }
        }


        public async Task<bool> UpdateRecurringItem(RecurringItem item)
        {
            try
            {
                using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
                await connection.OpenAsync();

                var parameters = new
                {
                    id = item.Id,
                    name = item.Name?.Trim() ?? string.Empty,
                    description = string.IsNullOrWhiteSpace(item.Description) ? (object)DBNull.Value : item.Description.Trim(),
                    categoryId = item.Category_catId,
                    minimumDue = item.MinimumDue,
                    balance = item.Balance.HasValue ? (object)item.Balance.Value : DBNull.Value,
                    interestRate = item.InterestRate.HasValue ? (object)(item.InterestRate.Value / 100) : DBNull.Value,
                    dayOfMonth = item.Day,
                    january = item.Jan,
                    february = item.Feb,
                    march = item.Mar,
                    april = item.Apr,
                    may = item.May,
                    june = item.Jun,
                    july = item.Jul,
                    august = item.Aug,
                    september = item.Sep,
                    october = item.Oct,
                    november = item.Nov,
                    december = item.Dec,
                    paidOff = item.PaidOff
                };

                var result = await connection.ExecuteAsync(
                    @"CALL spw_update_recurring_item(
                    @id, @name, @description, @categoryId, @minimumDue, @balance, @interestRate, @dayOfMonth,
                    @january, @february, @march, @april, @may, @june, @july, @august,
                    @september, @october, @november, @december, @paidOff
                    )",
                    parameters
                );

                return true;
            }
            catch (PostgresException pgEx)
            {
                System.Diagnostics.Debug.WriteLine($"PostgreSQL Error: {pgEx.MessageText}");
                System.Diagnostics.Debug.WriteLine($"Detail: {pgEx.Detail}");
                System.Diagnostics.Debug.WriteLine($"Hint: {pgEx.Hint}");
                System.Diagnostics.Debug.WriteLine($"Position: {pgEx.Position}");
                System.Diagnostics.Debug.WriteLine($"SqlState: {pgEx.SqlState}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in UpdateRecurringItem: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return false;
            }
        }


        public async Task<bool> DeleteRecurringItem(int itemId)
        {
            try
            {
                using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
                await connection.OpenAsync();

                var parameters = new
                {
                    itemId = itemId
                };

                await connection.ExecuteAsync(
                    "CALL spw_delete_recurring_item(@itemId)",
                    parameters
                );

                return true;
            }
            catch (PostgresException pgEx)
            {
                System.Diagnostics.Debug.WriteLine($"PostgreSQL Error: {pgEx.MessageText}");
                System.Diagnostics.Debug.WriteLine($"Detail: {pgEx.Detail}");
                System.Diagnostics.Debug.WriteLine($"Hint: {pgEx.Hint}");
                System.Diagnostics.Debug.WriteLine($"Position: {pgEx.Position}");
                System.Diagnostics.Debug.WriteLine($"SqlState: {pgEx.SqlState}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in DeleteRecurringItem: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return false;
            }
        }





        public async Task<IEnumerable<IncidentalItem>> GetIncidentalItems(int month, int year)
        {
            try
            {
                using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
                await connection.OpenAsync();

                var parameters = new
                {
                    month = month,
                    year = year
                };

                var items = (await connection.QueryAsync<IncidentalItem>(
                    "SELECT * FROM fnc_get_incidental_items_by_month(@month, @year)", parameters
                ))?.ToList();

                if (items == null || items.Count == 0)
                {
                    return new List<IncidentalItem> { new IncidentalItem() };
                }

                return items;
            }
            catch (PostgresException pgEx)
            {
                System.Diagnostics.Debug.WriteLine($"PostgreSQL Error: {pgEx.MessageText}");
                System.Diagnostics.Debug.WriteLine($"Detail: {pgEx.Detail}");
                System.Diagnostics.Debug.WriteLine($"Hint: {pgEx.Hint}");
                System.Diagnostics.Debug.WriteLine($"Position: {pgEx.Position}");
                System.Diagnostics.Debug.WriteLine($"SqlState: {pgEx.SqlState}");
                return new List<IncidentalItem> { new IncidentalItem() };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in GetIncidentalItems: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new List<IncidentalItem> { new IncidentalItem() };
            }
        }


        public async Task<IEnumerable<TransactionType>> GetIncidentalTransactionTypes()
        {
            try
            {
                using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
                await connection.OpenAsync();

                var transactions = (await connection.QueryAsync<TransactionType>(
                    "SELECT * FROM fnc_get_incidental_transaction_types()", new { }
                )).ToList();

                if (transactions == null || transactions.Count == 0)
                {
                    return new List<TransactionType> { new TransactionType() };
                }

                return transactions;
            }
            catch (PostgresException pgEx)
            {
                System.Diagnostics.Debug.WriteLine($"PostgreSQL Error: {pgEx.MessageText}");
                System.Diagnostics.Debug.WriteLine($"Detail: {pgEx.Detail}");
                System.Diagnostics.Debug.WriteLine($"Hint: {pgEx.Hint}");
                System.Diagnostics.Debug.WriteLine($"Position: {pgEx.Position}");
                System.Diagnostics.Debug.WriteLine($"SqlState: {pgEx.SqlState}");
                return new List<TransactionType> { new TransactionType() };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in GetIncidentalTransactionTypes: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new List<TransactionType> { new TransactionType() };
            }
        }


        public async Task<IEnumerable<Category>> GetIncidentalCategories()
        {

            try
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
            catch (PostgresException pgEx)
            {
                System.Diagnostics.Debug.WriteLine($"PostgreSQL Error: {pgEx.MessageText}");
                System.Diagnostics.Debug.WriteLine($"Detail: {pgEx.Detail}");
                System.Diagnostics.Debug.WriteLine($"Hint: {pgEx.Hint}");
                System.Diagnostics.Debug.WriteLine($"Position: {pgEx.Position}");
                System.Diagnostics.Debug.WriteLine($"SqlState: {pgEx.SqlState}");
                return new List<Category> { new() };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in GetIncidentalCategories: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new List<Category> { new() };
            }
        }


        public async Task<IEnumerable<Category>> GetCategoriesByTransactionId(int transactionId)
        {
            try
            {
                using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
                await connection.OpenAsync();

                var parameters = new
                {
                    transactionId = transactionId
                };

                var categories = (await connection.QueryAsync<Category>(
                    "SELECT * FROM fnc_get_categories_by_transaction_type_id(@transactionId)", parameters
                ))?.ToList();

                if (categories == null || categories.Count == 0)
                {
                    return new List<Category> { new Category() };
                }

                return categories;
            }
            catch (PostgresException pgEx)
            {
                System.Diagnostics.Debug.WriteLine($"PostgreSQL Error: {pgEx.MessageText}");
                System.Diagnostics.Debug.WriteLine($"Detail: {pgEx.Detail}");
                System.Diagnostics.Debug.WriteLine($"Hint: {pgEx.Hint}");
                System.Diagnostics.Debug.WriteLine($"Position: {pgEx.Position}");
                System.Diagnostics.Debug.WriteLine($"SqlState: {pgEx.SqlState}");
                return new List<Category> { new Category() };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in GetCategoriesByTransactionId: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new List<Category> { new Category() };
            }
        }


        public async Task<bool> UpdateIncidentalItem(IncidentalItem item, int month, int year)
        {
            try
            {
                using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
                await connection.OpenAsync();

                var parameters = new
                {
                    id = item.Id,
                    incId = item.Incidental_incId,
                    name = item.Name,
                    description = item.Description,
                    date = item.Date,
                    amount = item.Amount,
                    paid = item.IsPaid,
                    catId = item.Category_catID,
                    month = month,
                    year = year
                };

                var result = await connection.ExecuteAsync(
                    @"CALL spw_update_incidental_item(
                    @id, @incId, @name, @description, @date, @amount, @paid, @catId, @month, @year
                    )",
                    parameters
                );

                return true;
            }
            catch (PostgresException pgEx)
            {
                System.Diagnostics.Debug.WriteLine($"PostgreSQL Error: {pgEx.MessageText}");
                System.Diagnostics.Debug.WriteLine($"Detail: {pgEx.Detail}");
                System.Diagnostics.Debug.WriteLine($"Hint: {pgEx.Hint}");
                System.Diagnostics.Debug.WriteLine($"Position: {pgEx.Position}");
                System.Diagnostics.Debug.WriteLine($"SqlState: {pgEx.SqlState}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in UpdateIncidentalItem: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return false;
            }
        }


        public async Task<bool> DeleteIncidentalItem(int itemId)
        {
            try
            {
                using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
                await connection.OpenAsync();

                var parameters = new
                {
                    itemId = itemId
                };

                await connection.ExecuteAsync(
                    "CALL spw_delete_incidental_item(@itemId)",
                    parameters
                );

                return true;
            }
            catch (PostgresException pgEx)
            {
                System.Diagnostics.Debug.WriteLine($"PostgreSQL Error: {pgEx.MessageText}");
                System.Diagnostics.Debug.WriteLine($"Detail: {pgEx.Detail}");
                System.Diagnostics.Debug.WriteLine($"Hint: {pgEx.Hint}");
                System.Diagnostics.Debug.WriteLine($"Position: {pgEx.Position}");
                System.Diagnostics.Debug.WriteLine($"SqlState: {pgEx.SqlState}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in DeleteIncidentalItem: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return false;
            }
        }
    }
}
