using home_manager.Helpers;

namespace home_manager.Areas.BudgetManager.ViewModels
{
    public class BalanceDetail_VModel
    {
        public DateTime Date { get; set; } = TimeZoneHelper.LocalTime;
        public decimal CurrentBalance { get; set; } = 0.0M;
        public decimal SavingsBalance { get; set; } = 0.0M;
        public decimal BalanceBeforeNextPay { get; set; } = 0.0M;
    }
}
