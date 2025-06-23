using home_manager.Areas.BudgetManager.DTOs;
using home_manager.Areas.BudgetManager.Models;
using home_manager.Areas.BudgetManager.Repositories;
using home_manager.Areas.BudgetManager.ViewModels;
using home_manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace home_manager.Areas.BudgetManager.Controllers
{
    [Area("BudgetManager")]
    [Authorize]
    public class LedgerController : Controller
    {
        private readonly IBudgetManagerRepository _repository;

        public LedgerController(IBudgetManagerRepository repository)
        {
            _repository = repository;
        }


        [HttpGet("Ledger/{month?}/{year?}")]
        public async Task<IActionResult> Index(int? month = null, int? year = null)
        {
            var model = new AvailableLedgerDropdown_VModel();

            if (month == null || year == null)
            {
                model.SelectedLedger = (await _repository.GetLatestAvailableLedger());
            }
            else
            {
                model.SelectedLedger = (month.Value, year.Value);
            }

            model.LedgerMonths = (await _repository.GetAvailableLedgerMonths()).ToList();
            model.LedgerYears = (await _repository.GetAvailableLedgerYears()).ToList();

            return View(model);
        }


        public async Task<IActionResult> GetLedgerTable(int month, int year, int editableId)
        {
            var model = new LedgerDTO();

            model.Items = (await _repository.GetLedgerItemsByMonth(month, year)).ToList();

            var prevMonth = month == 1 ? 12 : --month;
            var prevYear = month == 12 ? --year : year;

            var balances = (await _repository.GetEndingBalances(prevMonth, prevYear));
            model.PreviousCheckingEndingBalance = balances.Item1;
            model.PreviousSavingsEndingBalance = balances.Item2;
            model.EditableItemId = editableId;

            return PartialView("_LedgerTable", model);
        }


        public async Task<IActionResult> UpdateLedgerItem([FromBody] LedgerItemDTO dto)
        {
            try
            {

                var Id = dto.Id;
                var Amount = dto.Amount;
                var IsPaid = dto.IsPaid;
                var Date = dto.Date;

                // Save the incidental item using the repository
                var result = await _repository.UpdateLedgerItem(Id, Amount, IsPaid, Date);

                if (result)
                    return Json(new { success = true });
                else
                    return BadRequest("Failed to save the ledger item.");
            }
            catch (Exception ex)
            {
                // Log the error as needed
                return BadRequest($"Error saving incidental expense: {ex.Message}");
            }
        }


        public async Task<IActionResult> CreateNewLedger()
        {

            var latestLedger = await _repository.GetLatestAvailableLedger();

            // Get new ledger month and year
            int originalMonth = latestLedger.Item1;
            int originalYear = latestLedger.Item2;

            int newMonth = originalMonth < 12 ? originalMonth + 1 : 1;
            int newYear = newMonth == 1 ? originalYear + 1 : originalYear;


            // Create new ledger and incidental records
            await _repository.InsertNewLedger(newMonth, newYear);

            return Json(new
            {
                success = true,
                month = newMonth,
                year = newYear
            });

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
