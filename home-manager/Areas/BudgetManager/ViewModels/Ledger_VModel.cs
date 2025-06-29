namespace home_manager.Areas.BudgetManager.ViewModels
{
    public class Ledger_VModel
    {
        public AvailableLedgerDropdown_VModel LedgerDropdown { get; set; } = new();
        public BalanceDetail_VModel BalanceDetail { get; set; } = new();
    }
}
