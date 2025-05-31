using Dapper;
using home_manager.Data;
using home_manager.Models;
using Npgsql;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static home_manager.Helpers.DropdownHelper;

namespace home_manager.Areas.BudgetManager.ViewModels
{
    public class IncidentalItems_VModel
    {

        private readonly DbConnectionService _dbConnection;

        public List<IncidentalItem> Items { get; set; } = new();

        [Required]
        public decimal TotalAmount { get; private set; } = 0.0M;

        public IncidentalItems_VModel(DbConnectionService dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public class IncidentalItem
        {
            [Required]
            public int Id { get; set; } = -1;

            [Required]
            public int IncidentalId { get; set; } = -1;

            [Required]
            public string Name { get; set; } = String.Empty;

            public string Description { get; set; } = String.Empty;

            [Required]
            public DateTime DateIncurred { get; set; } = DateTime.Now;

            [Required]
            public decimal Amount { get; set; } = 0.0M;

            [Required]
            public int CategoryId { get; set; } = -1;

            [Required]
            public string CategoryName { get; set; } = String.Empty;
        }

        public async Task LoadIncidentalItemsAsync(int month, int year)
        {
            using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
            await connection.OpenAsync();
            var items = (await connection.QueryAsync<IncidentalItem>(
                "SELECT * FROM fnc_get_incidental_items_by_month(@month, @year)", new { month, year }
            ))?.ToList();

            if (items == null || items.Count == 0)
            {
                Items = new List<IncidentalItem> { new IncidentalItem() };
                TotalAmount = 0.0M;
            }
            else
            {
                Items = items;
                TotalAmount = Items.Sum(item => item.Amount);
            }
        }
    }

    public class IncidentalDynamicRow_VModel
    {
        private readonly DbConnectionService _dbConnection;

        public List<DynamicCategoryOption> DynamicCategoryOptions { get; set; } = new();
        public List<DynamicTransactionOption> DynamicTransactionOptions { get; set; } = new();

        public IncidentalDynamicRow_VModel(DbConnectionService dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public class DynamicCategoryOption
        {
            public int Id { get; set; } = -1;

            public int TransactionId { get; set; } = -1;

            public string Description { get; set; } = String.Empty;

            
        }

        public class DynamicTransactionOption
        {
            public int Id { get; set; } = -1;

            public string Description { get; set; } = String.Empty;
        }

        public async Task GetDynamicOptions()
        {
            using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
            await connection.OpenAsync();
            var categories = (await connection.QueryAsync<DynamicCategoryOption>(
                "SELECT * FROM fnc_get_incidental_categories()", new { }
            ))?.ToList();

            var transactions = (await connection.QueryAsync<DynamicTransactionOption>(
                "SELECT * FROM fnc_get_incidental_transaction_types()", new { }
            )).ToList();

            if (categories == null || categories.Count == 0)
            {
                DynamicCategoryOptions = new List<DynamicCategoryOption> { new DynamicCategoryOption() };
            }
            else
            {
                DynamicCategoryOptions = categories;
            }

            if (transactions == null || transactions.Count == 0)
            {
                DynamicTransactionOptions = new List<DynamicTransactionOption> { new DynamicTransactionOption() };
            }
            else
            {
                DynamicTransactionOptions = transactions;
            }
        }
    }
}
