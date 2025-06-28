using home_manager.Areas.BudgetManager.Models;

namespace home_manager.Areas.BudgetManager.ViewModels
{
    public class SavingsItems_VModel
    {
        public List<SavingsLedgerItem> Items { get; set; } = new();
        public int EditableItemId { get; set; } = 0;
        public List<Category> Categories { get; set; } = new();
    }
}
