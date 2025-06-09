namespace home_manager.Areas.BudgetManager.DTOs
{
    public class LedgerItemDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime Date { get; set; }
    }
}
