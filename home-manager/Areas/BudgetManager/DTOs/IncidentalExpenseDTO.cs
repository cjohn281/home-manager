using home_manager.Areas.BudgetManager.Models;

namespace home_manager.Areas.BudgetManager.DTOs
{
    public class IncidentalExpenseDTO
    {
        public required IncidentalItem Model { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
