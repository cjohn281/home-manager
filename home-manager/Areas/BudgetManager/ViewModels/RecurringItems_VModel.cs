using Dapper;
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
        private readonly CultureInfo _cultureInfo = new("en-US");

        public List<RecurringItem> Items { get; set; } = new();

        [Required]
        public decimal TotalMinimumDue { get; private set; } = 0.0M;

        [Required]
        public decimal TotalBalance { get; private set; } = 0.0M;

        public class RecurringItem
        {
            [Required]
            public int Id { get; set; }

            [Required]
            public string Name { get; set; } = String.Empty;

            public string? Description { get; set; }

            [Required]
            public int CategoryId { get; set; }

            [Required]
            public string CategoryName { get; set; } = String.Empty;

            [Required]
            public decimal MinimumDue { get; set; }

            public decimal? Balance { get; set; }

            public decimal? InterestRate { get; set; }

            [Required]
            public int DayOfMonth { get; set; }

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
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="categoryId">The category ID to filter by. Use 0 for all categories.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task LoadRecurringItemsAsync(string connectionString, int categoryId)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            Items = (await connection.QueryAsync<RecurringItem>(
                $"SELECT * FROM fnc_get_recurring_items({categoryId});"
            )).ToList();

            TotalMinimumDue = Items.Sum(item => item.MinimumDue);
            TotalBalance = Items.Sum(item => item.Balance ?? 0);
        }
    }

    /// <summary>
    /// View model for managing recurring expense categories used in filters.
    /// </summary>
    public class RecurringCategoryFilterItems_VModel
    {
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
        /// <param name="connectionString">The database connection string.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task LoadRecurringCategoryFilterItems(string connectionString)
        {
            using var connection = new NpgsqlConnection(connectionString);
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
        /// <summary>
        /// Gets or sets the recurring item being modified.
        /// </summary>
        public RecItem Item = new RecItem();

        /// <summary>
        /// Loads a specific recurring item from the database for modification.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="itemId">The ID of the item to load.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task LoadItem(string connectionString, int itemId)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            var item = await connection.QueryFirstOrDefaultAsync<RecItem>(
                @"SELECT 
                    rci_id AS Id,
                    rci_name AS Name,
                    rci_description AS Description,
                    rci_category_cat_id AS CategoryId,
                    rci_minimum_due AS MinimumDue,
                    rci_balance AS Balance,
                    rci_interest_rate AS InterestRate,
                    rci_day AS DayOfMonth,
                    COALESCE(rci_jan, false) AS January,
                    COALESCE(rci_feb, false) AS February,
                    COALESCE(rci_mar, false) AS March,
                    COALESCE(rci_apr, false) AS April,
                    COALESCE(rci_may, false) AS May,
                    COALESCE(rci_jun, false) AS June,
                    COALESCE(rci_jul, false) AS July,
                    COALESCE(rci_aug, false) AS August,
                    COALESCE(rci_sep, false) AS September,
                    COALESCE(rci_oct, false) AS October,
                    COALESCE(rci_nov, false) AS November,
                    COALESCE(rci_dec, false) AS December,
                    COALESCE(rci_paid_off, false) AS PaidOff
                FROM tbl_recurring_item
                WHERE rci_id = @itemId;",
                new { itemId }
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

    /// <summary>
    /// Combined view model for the modify modal, containing both the item being modified
    /// and the available categories for selection.
    /// </summary>
    public class ModifyItemCombinedViewModel
    {
        /// <summary>
        /// Gets or sets the recurring item being modified.
        /// </summary>
        public ModifyItem_VModel ModifyItem { get; set; } = new();

        /// <summary>
        /// Gets or sets the available categories for selection.
        /// </summary>
        public RecurringCategoryFilterItems_VModel Categories { get; set; } = new();
    }
}
