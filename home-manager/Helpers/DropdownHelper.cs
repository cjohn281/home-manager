namespace home_manager.Helpers
{
    public static class DropdownHelper
    {
        public static Dictionary<int, string> MonthDropdownOptions = new Dictionary<int, string>
            {
                { 1, "January" },
                { 2, "February" },
                { 3, "March" },
                { 4, "April" },
                { 5, "May" },
                { 6, "June" },
                { 7, "July" },
                { 8, "August" },
                { 9, "September" },
                { 10, "October" },
                { 11, "November" },
                { 12, "December" }
            };

        public class Month
        {
            public int MonthNumber { get; set; }
            public string MonthName { get; set; }

            public Month(int monthNumber, string monthName)
            {
                MonthNumber = monthNumber;
                MonthName = monthName;
            }
        }
    }
}
