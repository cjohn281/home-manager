namespace home_manager.Areas.BudgetManager.ViewModels
{
    public class LookupValue_VModel
    {
        public int Id { get; set; } = 0;
        public int LookupType_lktId { get; set; } = 0;
        public string LookupTypeName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string? ExtendedValue1 { get; set; }
        public string? ExtendedValue2 { get; set; }
    }
}
