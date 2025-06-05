using Dapper;
using home_manager.Areas.BudgetManager.Repositories;
using home_manager.Areas.BudgetManager.ViewModels;
using home_manager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

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
    }
}
