using home_manager.Areas.BudgetManager.Repositories;
using home_manager.Areas.BudgetManager.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace home_manager.Areas.BudgetManager.Controllers
{
    public class IncomeDetailController : BudgetManagerController
    {

        private readonly IBudgetManagerRepository _repository;

        public IncomeDetailController(IBudgetManagerRepository repository)
        {
            _repository = repository;
        }
    
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetIncomeDetailTable(int editableId)
        {
            var model = new IncomeDetail_VModel();
            model.Items = (await _repository.GetIncomeDetails()).ToList();
            model.PayFrequencies = (await _repository.GetLookupValuesByType(7)).ToList();
            model.PersonList = (await _repository.GetPersonList()).ToList();
            model.EditableItemId = editableId;

            return PartialView("_IncomeDetailTable", model);
        }
    }
}
