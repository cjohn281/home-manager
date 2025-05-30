using Dapper;
using home_manager.Data;
using home_manager.Models;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace home_manager.Areas.BudgetManager.ViewModels
{
    /// <summary>
    /// View model for managing a collection of recurring expense items.
    /// Provides functionality to load and calculate totals for recurring items.
    /// </summary>
    public class RecurringItems_VModel
    {
        private readonly DbConnectionService _dbConnection;

        public List<RecurringItem> Items { get; set; } = new();

        [Required]
        public decimal TotalMinimumDue { get; private set; } = 0.0M;

        [Required]
        public decimal TotalBalance { get; private set; } = 0.0M;

        public RecurringItems_VModel(DbConnectionService dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public class RecurringItem
        {
            [Required]
            public int Id { get; set; } = -1;

            [Required]
            public string Name { get; set; } = String.Empty;

            public string? Description { get; set; } = null;

            [Required]
            public int CategoryId { get; set; } = 10;

            [Required]
            public string CategoryName { get; set; } = String.Empty;

            [Required]
            public decimal MinimumDue { get; set; } = 0.0M;

            public decimal? Balance { get; set; } = null;

            public decimal? InterestRate { get; set; } = null;

            [Required]
            public int DayOfMonth { get; set; } = DateTime.Now.Day;

            [Required]
            public bool January { get; set; } = false;

            [Required]
            public bool February { get; set; } = false;

            [Required]
            public bool March { get; set; } = false;

            [Required]
            public bool April { get; set; } = false;

            [Required]
            public bool May { get; set; } = false;

            [Required]
            public bool June { get; set; } = false;

            [Required]
            public bool July { get; set; } = false;

            [Required]
            public bool August { get; set; } = false;

            [Required]
            public bool September { get; set; } = false;

            [Required]
            public bool October { get; set; } = false;

            [Required]
            public bool November { get; set; } = false;

            [Required]
            public bool December { get; set; } = false;

            [Required]
            public bool PaidOff { get; set; } = false;
        }

        /// <summary>
        /// Loads recurring items from the database based on the specified category.
        /// Also calculates total minimum due and total balance.
        /// </summary>
        /// <param name="categoryId">The category ID to filter by. Use 0 for all categories.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task LoadRecurringItemsAsync(int categoryId)
        {
            using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
            await connection.OpenAsync();
            var items = (await connection.QueryAsync<RecurringItem>(
                "SELECT * FROM fnc_get_recurring_items_by_category(@categoryId)", new { categoryId }
            ))?.ToList();

            if (items == null || items.Count == 0)
            {
                Items = new List<RecurringItem> { new RecurringItem() };
            }
            else
            {
                Items = items;
            }

            TotalMinimumDue = Items.Sum(item => item.MinimumDue);
            TotalBalance = Items.Sum(item => item.Balance ?? 0);
        }
    }

    /// <summary>
    /// View model for managing recurring expense categories used in filters.
    /// </summary>
    public class RecurringCategoryFilterItems_VModel
    {
        private readonly DbConnectionService _dbConnection;

        public RecurringCategoryFilterItems_VModel(DbConnectionService dbConnection)
        {
            _dbConnection = dbConnection;
        }
        /// <summary>
        /// Gets or sets the list of available recurring expense categories.
        /// </summary>
        public List<RecurringCategory> Items { get; set; } = new();

        /// <summary>
        /// Represents a category for recurring expenses.
        /// </summary>
        public class RecurringCategory
        {
            /// <summary>
            /// Gets or sets the unique identifier for the category.
            /// </summary>
            [Required]
            public int CategoryId { get; set; }

            /// <summary>
            /// Gets or sets the name of the category.
            /// </summary>
            [Required]
            public string CategoryName { get; set; } = String.Empty;
        }

        /// <summary>
        /// Loads all available recurring expense categories from the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task LoadRecurringCategoryFilterItems()
        {
            using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
            await connection.OpenAsync();
            Items = (await connection.QueryAsync<RecurringCategory>(
                "SELECT * FROM fnc_get_recurring_category_filter_items();"
            )).ToList();
        }
    }

    /// <summary>
    /// View model for modifying a single recurring expense item.
    /// </summary>
    public class ModifyItem_VModel
    {
        private readonly DbConnectionService _dbConnection;

        public ModifyItem_VModel(DbConnectionService dbConnection)
        {
            _dbConnection = dbConnection;
        }

        /// <summary>
        /// Gets or sets the recurring item being modified.
        /// </summary>
        public RecItem Item = new RecItem();

        /// <summary>
        /// Loads a specific recurring item from the database for modification.
        /// </summary>
        /// <param name="itemId">The ID of the item to load.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task LoadItem(int itemId)
        {
            using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
            await connection.OpenAsync();
            var item = await connection.QueryFirstOrDefaultAsync<RecItem>(
                "SELECT * FROM fnc_get_recurring_item_by_id(@itemId)", new { itemId }
            );

            if (item != null)
            {
                Item = item;
            }
        }

        public class RecItem
        {
            public int Id { get; set; } = 0;
            public string Name { get; set; } = String.Empty;
            public string Description { get; set; } = String.Empty;
            public int CategoryId { get; set; } = 10;
            public decimal MinimumDue { get; set; } = 0.0M;
            public decimal? Balance { get; set; } = null;
            public decimal? InterestRate { get; set; } = null;
            public int DayOfMonth { get; set; } = DateTime.Now.Day;
            public bool January { get; set; } = false;
            public bool February { get; set; } = false;
            public bool March { get; set; } = false;
            public bool April { get; set; } = false;
            public bool May { get; set; } = false;
            public bool June { get; set; } = false;
            public bool July { get; set; } = false;
            public bool August { get; set; } = false;
            public bool September { get; set; } = false;
            public bool October { get; set; } = false;
            public bool November { get; set; } = false;
            public bool December { get; set; } = false;
            public bool PaidOff { get; set; } = false;
        }
    }


    // Update the initialization of ModifyItem property in ModifyItemCombinedViewModel to pass the required parameter.

    public class ModifyItemCombinedViewModel
    {
        private readonly DbConnectionService _dbConnection;

        public ModifyItemCombinedViewModel(DbConnectionService dbConnection)
        {
            _dbConnection = dbConnection;
            ModifyItem = new ModifyItem_VModel(_dbConnection);
            Categories = new RecurringCategoryFilterItems_VModel(_dbConnection);
        }

        public ModifyItem_VModel ModifyItem { get; set; }

        public RecurringCategoryFilterItems_VModel Categories { get; set; }
    }
}
