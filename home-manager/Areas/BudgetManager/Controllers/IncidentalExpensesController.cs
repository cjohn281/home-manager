using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace home_manager.Areas.BudgetManager.Controllers
{
    [Area("BudgetManager")]
    [Authorize]
    public class IncidentalExpensesController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public IncidentalExpensesController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }


        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetIncidentalExpensesTable()
        {

            return View();
        }
    }
}
