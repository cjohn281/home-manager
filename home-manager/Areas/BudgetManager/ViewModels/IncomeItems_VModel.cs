using home_manager.Areas.BudgetManager.Models;

namespace home_manager.Areas.BudgetManager.ViewModels
{
    public class IncomeItems_VModel
    {
        public List<IncomeLedgerItem> Items { get; set; } = new();

        public int EditableItemId { get; set; } = 0;
    }
}
