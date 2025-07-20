using home_manager.Areas.BudgetManager.DTOs;
using home_manager.Areas.BudgetManager.Models;
using home_manager.Areas.BudgetManager.Repositories;
using home_manager.Areas.BudgetManager.ViewModels;
using home_manager.Helpers;
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
                if (await _repository.LedgerExists(TimeZoneHelper.LocalTime.Month, TimeZoneHelper.LocalTime.Year))
                {
                    model.SelectedLedger = (TimeZoneHelper.LocalTime.Month, TimeZoneHelper.LocalTime.Year);
                }
                else
                {
                    model.SelectedLedger = (await _repository.GetLatestAvailableLedger());
                }
                model.hideRunningBalance = false;
            }
            else
            {
                model.SelectedLedger = (month.Value, year.Value);
                if (year.Value > TimeZoneHelper.LocalTime.Year || (year.Value == TimeZoneHelper.LocalTime.Year && month.Value >= TimeZoneHelper.LocalTime.Month))
                {
                    model.hideRunningBalance = false;
                }
            }

            model.LedgerMonths = (await _repository.GetAvailableLedgerMonths()).ToList();
            model.LedgerYears = (await _repository.GetAvailableLedgerYears()).ToList();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetBalanceDetailCard()
        {
            var model = new BalanceDetail_VModel();
            model.Date = TimeZoneHelper.LocalTime;

            var balances = (await _repository.GetBalanceDetails(model.Date.Month, model.Date.Year));
            model.CurrentBalance = balances.Item1;
            model.SavingsBalance = balances.Item2;

            model.BalanceBeforeNextPay = (await _repository.GetBalanceBeforeNextPay(model.Date.Month, model.Date.Year));

            return PartialView("_BalanceDetailCard", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetLedgerTable(int month, int year, int editableId)
        {
            var model = new LedgerDTO();

            model.Items = (await _repository.GetLedgerItemsByMonth(month, year)).ToList();

            var prevMonth = month == 1 ? 12 : month - 1;
            var prevYear = prevMonth == 12 ? year - 1 : year;

            var balances = (await _repository.GetEndingBalances(prevMonth, prevYear));
            model.PreviousCheckingEndingBalance = balances.Item1;
            model.PreviousSavingsEndingBalance = balances.Item2;
            model.EditableItemId = editableId;

            if (year > TimeZoneHelper.LocalTime.Year || (year == TimeZoneHelper.LocalTime.Year && month >= TimeZoneHelper.LocalTime.Month))
                model.hideRunningBalance = false;

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
