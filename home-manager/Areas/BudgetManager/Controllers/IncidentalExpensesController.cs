using Dapper;
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
            await model.GetDynamicOptions();
            return PartialView("_IncidentalExpensesTable", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriesByTransaction(int transactionId)
        {
            try
            {
                if (string.IsNullOrEmpty(_dbConnection.GetConnectionString()))
                {
                    return BadRequest("Database connection string is not configured.");
                }

                using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
                await connection.OpenAsync();

                var categories = await connection.QueryAsync<dynamic>(
                    "SELECT cat_id as id, cat_description as description " +
                    "FROM tbl_category " +
                    "WHERE cat_transaction_type_tst_id = @transactionId " +
                    "ORDER BY cat_description",
                    new { transactionId }
                );

                return Json(categories);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error loading categories: {ex.Message}");
            }
        }
    }
}
