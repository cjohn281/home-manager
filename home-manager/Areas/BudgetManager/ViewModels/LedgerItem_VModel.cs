using Dapper;
using home_manager.Data;
using home_manager.Helpers;
using home_manager.Models;
using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace home_manager.Areas.BudgetManager.ViewModels
{
    public class LedgerItem_VModel
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 0.0M;
        public int TransactionType_tstId { get; set; } = 0;
        public string TransactionTypeName { get; set; } = string.Empty;
        public int Category_catId { get; set; } = 0;
        public string CategoryName { get; set; } = string.Empty;
        public DateTime Date { get; set; } = TimeZoneHelper.LocalTime;
        public bool IsPaid { get; set; } = true;
        public string ItemType { get; set; } = string.Empty;
    }

    public class AvailableLedgerDropdown_VModel
    {

        public (int month, int year) SelectedLedger { get; set; }

        public List<int> LedgerMonths { get; set; } = new();

        public List<int> LedgerYears { get; set; } = new();

        public bool hideRunningBalance { get; set; } = true;
    }
}
