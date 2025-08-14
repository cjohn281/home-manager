namespace home_manager.Areas.BudgetManager.Models
{
    public class IncomeDetail
    {
        public int Id { get; set; } = 0;
        public int Person_prnId { get; set; } = 0;
        public string PersonName { get; set; } = string.Empty;
        public decimal DefaultAmount { get; set; } = 0.0M;
        public int PayFrequency_lvlId { get; set; } = 0;
        public string PayFrequency { get; set; } = string.Empty;
        public int? PayDate1 { get; set; }
        public int? PayDate2 { get; set; }
        public DateTime? ValidDate { get; set; }
    }
}
