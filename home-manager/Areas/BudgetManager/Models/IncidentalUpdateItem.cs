namespace home_manager.Areas.BudgetManager.Models
{
    public class IncidentalUpdateItem
    {
        public IncidentalItem IncidentalItem { get; set; } = new();
        public int month { get; set; } = 0;
        public int year { get; set; } = 0;
    }
}
