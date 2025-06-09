using home_manager.Areas.BudgetManager.ViewModels;

namespace home_manager.Areas.BudgetManager.DTOs
{
    public class LedgerDTO
    {
        public List<LedgerItem_VModel> Items { get; set; } = new();
        public decimal CheckingStartingBalance { get; set; } = 0.0M;
        public decimal SavingsStartingBalance { get; set; } = 0.0M;
        public int EditableItemId { get; set; } = 0;
    }
}
