using Dapper;
using home_manager.Areas.BudgetManager.DTOs;
using home_manager.Areas.BudgetManager.Models;
using home_manager.Areas.BudgetManager.Repositories;
using home_manager.Areas.BudgetManager.ViewModels;
using home_manager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Diagnostics;

namespace home_manager.Areas.BudgetManager.Controllers
{
    [Area("BudgetManager")]
    [Authorize]
    public class IncidentalExpensesController : Controller
    {
        private readonly IBudgetManagerRepository _repository;

        public IncidentalExpensesController(IBudgetManagerRepository repository)
        {
            _repository = repository;
        }


        public async Task<IActionResult> Index()
        {
            var model = new AvailableLedgerDropdown_VModel();

            model.LatestAvailableLedger = (await _repository.GetLatestAvailableLedger());
            model.LedgerMonths = (await _repository.GetAvailableLedgerMonthsAsync()).ToList();
            model.LedgerYears = (await _repository.GetAvailableLedgerYearsAsync()).ToList();
            
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetIncidentalExpensesTable(int month, int year)
        {
            var model = new IncidentalItems_VModel();

            model.Items = (await _repository.GetIncidentalItemsAsync(month, year)).ToList();
            model.DynamicCategoryOptions = (await _repository.GetIncidentalCategoriesAsync()).ToList();
            model.DynamicTransactionOptions = (await _repository.GetIncidentalTransactionTypesAsync()).ToList();
            model.CalculateTotals();

            return PartialView("_IncidentalExpensesTable", model);
        }


        [HttpGet]
        public async Task<IActionResult> GetCategoriesByTransaction(int transactionId)
        {

            var categories = (await _repository.GetCategoriesByTransactionId(transactionId)).ToList();

            return Json(categories);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateIncidentalExpense([FromBody] IncidentalExpenseDTO dto)
        {

            var model = dto.Model;
            var month = dto.Month;
            var year = dto.Year;

            Debug.WriteLine(month.ToString());
            Debug.WriteLine(year.ToString());
            Debug.WriteLine(model.Name);

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
    }
}
