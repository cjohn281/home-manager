using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace home_manager.Areas.BudgetManager.Controllers
{
    [Area("BudgetManager")]
    public class RecurringItemsController : Controller
    {
        private readonly IConfiguration _configuration;

        public RecurringItemsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var model = new ViewModels.RecurringItems_VModel();
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                // Handle the case where the connection string is null or empty
                return BadRequest("Database connection string is not configured.");
            }

            await model.LoadItemsAsync(connectionString);
            return View(model);
        }
    }
}
