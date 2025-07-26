using home_manager.Areas.BudgetManager.Models;
using home_manager.Areas.BudgetManager.Repositories;
using home_manager.Areas.BudgetManager.ViewModels;
using home_manager.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static home_manager.Helpers.DropdownHelper;

namespace home_manager.Areas.BudgetManager.Controllers
{
    public class IncomeController : BudgetManagerController
    {
        private readonly IBudgetManagerRepository _repository;

        public IncomeController(IBudgetManagerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("Income/{month?}/{year?}")]
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
        public async Task<IActionResult> GetIncomeTable(int month, int year, int editableId)
        {
            var model = new IncomeItems_VModel();
            model.Items = (await _repository.GetIncomeLedgerItems(month, year)).ToList();
            model.EditableItemId = editableId;

            return PartialView("_IncomeTable", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateIncomeLedgerItem([FromBody] IncomeLedgerItem item)
        {
            try
            {
                // Save the incidental item using the repository
                var result = await _repository.UpdateIncomeLedgerItem(item);

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
    }
}
