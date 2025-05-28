using Dapper;
using home_manager.Areas.BudgetManager.ViewModels;
using home_manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace home_manager.Areas.BudgetManager.Controllers
{
    /// <summary>
    /// Controller for managing recurring expenses in the Budget Manager area.
    /// Handles CRUD operations for recurring expense items and their categories.
    /// </summary>
    [Area("BudgetManager")]
    [Authorize]
    public class RecurringExpensesController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the RecurringExpensesController.
        /// </summary>
        /// <param name="configuration">The application configuration containing connection strings.</param>
        public RecurringExpensesController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        /// <summary>
        /// Displays the main index view for recurring expenses, including category filters.
        /// </summary>
        /// <returns>
        /// Returns the index view with category filter options.
        /// Returns BadRequest if the database connection string is not configured.
        /// </returns>
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var model = new RecurringCategoryFilterItems_VModel();

            if (string.IsNullOrEmpty(_connectionString))
            {
                return BadRequest("Database connection string is not configured.");
            }

            await model.LoadRecurringCategoryFilterItems(_connectionString);
            return View(model);
        }

        /// <summary>
        /// Retrieves and returns a partial view containing the recurring expenses table filtered by category.
        /// </summary>
        /// <param name="categoryId">The ID of the category to filter by. Use 0 for all categories.</param>
        /// <returns>
        /// Returns a partial view with the filtered recurring expenses table.
        /// Returns BadRequest if an error occurs or if the connection string is not configured.
        /// </returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetRecurringExpensesTable(int categoryId)
        {
            try
            {
                var model = new RecurringItems_VModel();

                if (string.IsNullOrEmpty(_connectionString))
                {
                    return BadRequest("Database connection string is not configured.");
                }

                await model.LoadRecurringItemsAsync(_connectionString, categoryId);
                return PartialView("_RecurringExpensesTable", model);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error loading recurring expenses: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the modify modal partial view for editing a recurring expense item.
        /// </summary>
        /// <param name="itemId">The ID of the recurring expense item to edit.</param>
        /// <returns>
        /// Returns a partial view containing the modal form for editing the specified item.
        /// Returns BadRequest if the connection string is not configured.
        /// </returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LoadModifyModal(int itemId)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                return BadRequest("Database connection string is not configured.");
            }

            var model = new ModifyItemCombinedViewModel
            {
                ModifyItem = new ModifyItem_VModel { Item = new ModifyItem_VModel.RecItem() },
                Categories = new RecurringCategoryFilterItems_VModel()
            };
            await model.ModifyItem.LoadItem(_connectionString, itemId);
            await model.Categories.LoadRecurringCategoryFilterItems(_connectionString);

            return PartialView("_ModifyModal", model);
        }

        /// <summary>
        /// Updates an existing recurring expense item with the provided data.
        /// </summary>
        /// <param name="item">The recurring item data containing the updates.</param>
        /// <returns>
        /// Returns a JSON result indicating success and the category ID for table refresh.
        /// Returns BadRequest if validation fails or if an error occurs during update.
        /// </returns>
        /// <remarks>
        /// The method handles null values for Balance and InterestRate by converting 0 to null.
        /// InterestRate is converted from percentage to decimal format before storage.
        /// </remarks>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateRecurringItem([FromBody] ModifyItem_VModel.RecItem item)
        {
            try
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    return BadRequest("Database connection string is not configured.");
                }

                // Validate required fields
                if (item == null)
                    return BadRequest("No data provided");
                if (string.IsNullOrWhiteSpace(item.Name))
                    return BadRequest("Name is required");
                if (item.CategoryId == 0)
                    return BadRequest("Category is required");
                if (item.MinimumDue < 0)
                    return BadRequest("Minimum Due cannot be negative");

                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                await connection.ExecuteAsync(
                    "CALL spw_update_recurring_item(@id, @name, @description, @categoryId, @minimumDue, @balance, @interestRate, @dayOfMonth, " +
                    "@january, @february, @march, @april, @may, @june, @july, @august, @september, @october, @november, @december, @paidOff)",
                    new
                    {
                        id = item.Id,
                        name = item.Name.Trim(),
                        description = string.IsNullOrWhiteSpace(item.Description) ? null : item.Description.Trim(),
                        categoryId = item.CategoryId,
                        minimumDue = item.MinimumDue,
                        balance = item.Balance.HasValue && item.Balance.Value != 0 ? item.Balance : null,
                        interestRate = item.InterestRate.HasValue && item.InterestRate.Value != 0 ?
                            (decimal?)(item.InterestRate.Value / 100) : null,
                        dayOfMonth = item.DayOfMonth,
                        january = item.January,
                        february = item.February,
                        march = item.March,
                        april = item.April,
                        may = item.May,
                        june = item.June,
                        july = item.July,
                        august = item.August,
                        september = item.September,
                        october = item.October,
                        november = item.November,
                        december = item.December,
                        paidOff = item.PaidOff
                    });

                return Json(new { success = true, categoryId = item.CategoryId });
            }
            catch (PostgresException pex)
            {
                return BadRequest($"Database error: {pex.MessageText}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating recurring item: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteRecurringItem(int itemId)
        {
            try
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    return BadRequest("Database connection string is not configured.");
                }
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                await connection.ExecuteAsync("CALL spw_delete_recurring_item(@itemId)", new { itemId });
                return Json(new { success = true });
            }
            catch (PostgresException pex)
            {
                return BadRequest($"Database error: {pex.MessageText}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting recurring item: {ex.Message}");
            }
        }
    }
}
