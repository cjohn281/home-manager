using home_manager.Helpers;
using home_manager.Areas.BudgetManager.DTOs;
using home_manager.Areas.BudgetManager.Repositories;
using home_manager.Areas.BudgetManager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using home_manager.Areas.BudgetManager.Models;

namespace home_manager.Areas.BudgetManager.Controllers
{
    public class IncidentalExpensesController : BudgetManagerController
    {
        private readonly IBudgetManagerRepository _repository;

        public IncidentalExpensesController(IBudgetManagerRepository repository)
        {
            _repository = repository;
        }


        [HttpGet("IncidentalExpenses/{month?}/{year?}")]
        public async Task<IActionResult> Index(int? month = null, int? year = null)
        {
            var model = new AvailableLedgerDropdown_VModel();

            if (month == null || year == null)
            {
                if (await _repository.LedgerExists(TimeZoneHelper.LocalTime.Month, TimeZoneHelper.LocalTime.Year))
                {
                    model.SelectedLedger = (TimeZoneHelper.LocalTime.Month, TimeZoneHelper.LocalTime.Year);
                }
                else
                {
                    model.SelectedLedger = (await _repository.GetLatestAvailableLedger());
                }
            }
            else
            {
                model.SelectedLedger = (month.Value, year.Value);
            }

            model.LedgerMonths = (await _repository.GetAvailableLedgerMonths()).ToList();
            model.LedgerYears = (await _repository.GetAvailableLedgerYears()).ToList();
            
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetIncidentalExpensesTable(int month, int year, int editableId)
        {
            var model = new IncidentalItems_VModel();

            model.Items = (await _repository.GetIncidentalItems(month, year)).ToList();
            model.DynamicCategoryOptions = (await _repository.GetIncidentalCategories()).ToList();
            //model.DynamicTransactionOptions = (await _repository.GetIncidentalTransactionTypes()).ToList();
            model.EditableItemId = editableId;
            model.CalculateTotals();

            if (month == 1 && year == 2025)
                model.ShowDynamicRow = false;

            return PartialView("_IncidentalExpensesTable", model);
        }


        [HttpGet]
        public async Task<IActionResult> GetCategoriesByTransaction(int transactionId, int itemId)
        {
            IncidentalItem selectedItem = new();
            int selectedCategory = 0;

            var categories = (await _repository.GetCategoriesByTransactionId(transactionId)).ToList();

            if (itemId != 0)
            {
                selectedItem = (await _repository.GetIncidentalItemById(itemId));
                selectedCategory = selectedItem.Category_catID;
            }


            return Json(new {
                Categories = categories.Select(c => new { id = c.Id, description = c.Description }),
                SelectedCategory = selectedCategory
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateIncidentalExpense([FromBody] IncidentalExpenseDTO dto)
        {

            var model = dto.Model;
            var month = dto.Month;
            var year = dto.Year;

            if (model == null)
                return BadRequest("No data provided.");

            // Basic validation
            if (string.IsNullOrWhiteSpace(model.Name))
                return BadRequest("Expense is required.");
            if (model.Category_catID == 0)
                return BadRequest("Category is required.");
            if (month == 0 || year == 0)
                return BadRequest("Month and year are required.");

            try
            {
                // Save the incidental item using the repository
                var result = await _repository.UpdateIncidentalItem(model, month, year);

                if (result)
                    return Json(new { success = true });
                else
                    return BadRequest("Failed to save the incidental expense.");
            }
            catch (Exception ex)
            {
                // Log the error as needed
                return BadRequest($"Error saving incidental expense: {ex.Message}");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteIncidentalItem(int itemId)
        {
            try
            {
                var success = await _repository.DeleteIncidentalItem(itemId);

                if (success)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return BadRequest("Failed to delete incidental item");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting incidental item: {ex.Message}");
            }
        }

        public async Task<IActionResult> LoadModifyModal(int itemID)
        {
            var model = await _repository.GetIncidentalItemById(itemID);
            model.CategoryList = (await _repository.GetIncidentalCategories()).ToList<Category>();

            return PartialView("_ModifyModal", model);
        }
    }
}
