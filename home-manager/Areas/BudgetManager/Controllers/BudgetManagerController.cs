using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace home_manager.Areas.BudgetManager.Controllers
{
    [Area("BudgetManager")]
    [Authorize(Roles = "Administrator,Elevated")]
    public abstract class BudgetManagerController : Controller
    {
    }
}
