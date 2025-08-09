using home_manager.Areas.BudgetManager.Models;
using home_manager.Areas.BudgetManager.Repositories;
using home_manager.Areas.BudgetManager.ViewModels;
using home_manager.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace home_manager.Areas.BudgetManager.Controllers
{
    public class SavingsController : BudgetManagerController
    {
        private readonly IBudgetManagerRepository _repository;

        public SavingsController(IBudgetManagerRepository repository)
        {
            _repository = repository;
        }


        [HttpGet("Savings/{month?}/{year?}")]
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
        public async Task<IActionResult> GetSavingsTable(int month, int year, int editableId)
        {
            var model = new SavingsItems_VModel();
            model.Items = (await _repository.GetSavingsLedgerItems(month, year)).ToList();
            model.Categories = (await _repository.GetCategoriesByTransactionId(5)).ToList();
            model.EditableItemId = editableId;

            return PartialView("_SavingsTable", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSavingsLedgerItem([FromBody] SavingsLedgerItem item)
        {
            try
            {
                // Save the incidental item using the repository
                var result = await _repository.UpdateSavingsLedgerItem(item);

                if (result)
                    return Json(new { success = true });
                else
                    return BadRequest("Failed to save the income ledger item.");
            }
            catch (Exception ex)
            {
                // Log the error as needed
                return BadRequest($"Error saving income ledger item: {ex.Message}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSavingsItem(int itemId)
        {
            try
            {
                var success = await _repository.DeleteSavingsLedgerItem(itemId);

                if (success)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return BadRequest("Failed to delete savings ledger item");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting savings ledger item: {ex.Message}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadModifyModal(int itemId)
        {
            var model = await _repository.GetSavingsLedgerItemById(itemId);
            model.CategoryList = (await _repository.GetCategoriesByTransactionId(5)).ToList();

            return PartialView("_ModifyModal", model);
        }
    }
}
