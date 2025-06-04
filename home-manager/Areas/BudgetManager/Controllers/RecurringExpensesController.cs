using Dapper;
using home_manager.Areas.BudgetManager.Models;
using home_manager.Areas.BudgetManager.Repositories;
using home_manager.Areas.BudgetManager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
        private readonly IBudgetManagerRepository _repository;

        /// <summary>
        /// Initializes a new instance of the RecurringExpensesController.
        /// </summary>
        /// <param name="configuration">The application configuration containing connection strings.</param>
        public RecurringExpensesController(IBudgetManagerRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Displays the main index view for recurring expenses, including category filters.
        /// </summary>
        /// <returns>
        /// Returns the index view with category filter options.
        /// Returns BadRequest if the database connection string is not configured.
        /// </returns>
        public async Task<IActionResult> Index()
        {
            var model = new RecurringCategoryFilterItems_VModel();
            model.Categories = (await _repository.GetRecurringCategoryFilterItemsAsync()).ToList();
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetRecurringExpensesTable(int categoryId)
        {
            var model = new RecurringItems_VModel();
            model.Items = (await _repository.GetRecurringItemsByCategoryIdAsync(categoryId)).ToList();
            var categories = (await _repository.GetRecurringCategoryFilterItemsAsync()).ToList();
            model.CategoryNames = categories.ToDictionary(c => c.Id, c => c.Description);
            model.CalculateTotals();

            foreach (var item in model.Items)
            {
                Debug.WriteLine($"Item: {item.Name}, Category_catId: {item.Category_catId}, Category: {model.GetCategoryName(item)}");
            }

            return PartialView("_RecurringExpensesTable", model);
        }

        ///// <summary>
        ///// Loads the modify modal partial view for editing a recurring expense item.
        ///// </summary>
        ///// <param name="itemId">The ID of the recurring expense item to edit.</param>
        ///// <returns>
        ///// Returns a partial view containing the modal form for editing the specified item.
        ///// Returns BadRequest if the connection string is not configured.
        ///// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadModifyModal(int itemId)
        {
            var model = new ModifyItemCombinedViewModel()
            {
                ModifyItem = new ModifyItem_VModel() { Item = await _repository.GetRecurringItemByIdAsync(itemId) },
                Categories = new RecurringCategoryFilterItems_VModel() { Categories = (await _repository.GetRecurringCategoryFilterItemsAsync()).ToList() }
            };

            return PartialView("_ModifyModal", model);
        }

        ///// <summary>
        ///// Updates an existing recurring expense item with the provided data.
        ///// </summary>
        ///// <param name="item">The recurring item data containing the updates.</param>
        ///// <returns>
        ///// Returns a JSON result indicating success and the category ID for table refresh.
        ///// Returns BadRequest if validation fails or if an error occurs during update.
        ///// </returns>
        ///// <remarks>
        ///// The method handles null values for Balance and InterestRate by converting 0 to null.
        ///// InterestRate is converted from percentage to decimal format before storage.
        ///// </remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRecurringItem([FromBody] RecurringItem item)
        {
            try
            {
                // Validate required fields
                if (item == null)
                    return BadRequest("No data provided");
                if (string.IsNullOrWhiteSpace(item.Name))
                    return BadRequest("Name is required");
                if (item.Category_catId == 0)
                    return BadRequest("Category is required");
                if (item.MinimumDue < 0)
                    return BadRequest("Minimum Due cannot be negative");

                // Map the view model to domain model
                var recurringItem = new RecurringItem
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Category_catId = item.Category_catId,
                    MinimumDue = item.MinimumDue,
                    Balance = item.Balance,
                    InterestRate = item.InterestRate,
                    Day = item.Day,
                    Jan = item.Jan,
                    Feb = item.Feb,
                    Mar = item.Mar,
                    Apr = item.Apr,
                    May = item.May,
                    Jun = item.Jun,
                    Jul = item.Jul,
                    Aug = item.Aug,
                    Sep = item.Sep,
                    Oct = item.Oct,
                    Nov = item.Nov,
                    Dec = item.Dec,
                    PaidOff = item.PaidOff
                };

                var success = await _repository.UpdateRecurringItem(recurringItem);

                if (success)
                {
                    return Json(new { success = true, categoryId = item.Category_catId });
                }
                else
                {
                    return BadRequest("Failed to update recurring item");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating recurring item: {ex.Message}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRecurringItem(int itemId)
        {
            try
            {
                var success = await _repository.DeleteRecurringItemAsync(itemId);

                if (success)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return BadRequest("Failed to delete recurring item");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting recurring item: {ex.Message}");
            }
        }
    }
}
