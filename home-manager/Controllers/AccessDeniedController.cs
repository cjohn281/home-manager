using Microsoft.AspNetCore.Mvc;

namespace home_manager.Controllers
{
    public class AccessDeniedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
