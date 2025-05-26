using System.Globalization;

namespace home_manager.Helpers
{
    public static class FormatHelper
    {
        private static readonly CultureInfo _cultureInfo = new("en-US");

        public static string FormatCurrency(decimal amount)
        {
            return amount.ToString("C", _cultureInfo);
        }

    }
}
