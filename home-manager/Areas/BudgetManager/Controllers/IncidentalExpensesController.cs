using home_manager.Areas.BudgetManager.ViewModels;
using home_manager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace home_manager.Areas.BudgetManager.Controllers
{
    [Area("BudgetManager")]
    [Authorize]
    public class IncidentalExpensesController : Controller
    {
        private readonly DbConnectionService _dbConnection;

        public IncidentalExpensesController(DbConnectionService dbConnection)
        {
            _dbConnection = dbConnection;
        }


        public async Task<IActionResult> Index()
        {
            var model = new AvailableLedgerDropdown_VModel(_dbConnection);

            if (string.IsNullOrEmpty(_dbConnection.GetConnectionString()))
            {
                return BadRequest("Database connection string is not configured.");
            }

            await model.LoadAvailableLedgerDropdowns();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetIncidentalExpensesTable(int month, int year)
        {
            var model = new IncidentalItems_VModel(_dbConnection);

            if (string.IsNullOrEmpty(_dbConnection.GetConnectionString()))
            {
                return BadRequest("Database connection string is not configured.");
            }

            await model.LoadIncidentalItemsAsync(month, year);
            return PartialView("_IncidentalExpensesTable", model);
        }
    }
}
