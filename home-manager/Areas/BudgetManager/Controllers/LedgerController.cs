using home_manager.Areas.BudgetManager.DTOs;
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

            Debug.WriteLine($"Month: {model.SelectedLedger.month}, year: {model.SelectedLedger.year}");

            return View(model);
        }


        public async Task<IActionResult> GetLedgerTable(int month, int year, int editableId)
        {
            var model = new LedgerDTO();

            model.Items = (await _repository.GetLedgerItemsByMonth(month, year)).ToList();
            var balances = (await _repository.GetStartingBalances(month, year));
            model.CheckingStartingBalance = balances.Item1;
            model.SavingsStartingBalance = balances.Item2;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
