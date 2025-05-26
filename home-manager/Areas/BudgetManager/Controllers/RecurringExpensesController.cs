using home_manager.Areas.BudgetManager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace home_manager.Areas.BudgetManager.Controllers
{
    [Area("BudgetManager")]
    [Authorize]
    public class RecurringExpensesController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public RecurringExpensesController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var model = new RecurringCategoryFilterItems_VModel();

            if (string.IsNullOrEmpty(_connectionString))
            {
                // Handle the case where the connection string is null or empty
                return BadRequest("Database connection string is not configured.");
            }

            await model.LoadRecurringCategoryFilterItems(_connectionString);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetRecurringExpensesTable(int categoryId)
        {
            var model = new RecurringItems_VModel();

            if (string.IsNullOrEmpty(_connectionString))
            {
                // Handle the case where the connection string is null or empty
                return BadRequest("Database connection string is not configured.");
            }

            await model.LoadRecurringItemsAsync(_connectionString, categoryId);

            return PartialView("_RecurringExpensesTable", model);
        }
    }
}
