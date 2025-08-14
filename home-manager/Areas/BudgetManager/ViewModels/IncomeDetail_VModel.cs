using home_manager.Areas.BudgetManager.Models;
using home_manager.Areas.BudgetManager.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.InteropServices;

namespace home_manager.Areas.BudgetManager.ViewModels
{
    public class IncomeDetail_VModel
    {
        public List<IncomeDetail> Items { get; set; } = new List<IncomeDetail> { new IncomeDetail() };
        public List<LookupValue_VModel> PayFrequencies { get; set; } = new List<LookupValue_VModel> { new LookupValue_VModel() };
        public List<Person> PersonList { get; set; } = new List<Person> { new Person() };
        public int EditableItemId { get; set; } = 0;
        public bool ShowDynamicRow { get; set; } = true;
    }
}
