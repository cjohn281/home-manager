using Dapper;
using home_manager.Data;
using home_manager.Models;
using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace home_manager.Areas.BudgetManager.ViewModels
{
    public class Ledger_VModel
    {

    }

    public class AvailableLedgerDropdown_VModel
    {

        public (int month, int year) LatestAvailableLedger { get; set; }

        public List<int> LedgerMonths { get; set; } = new();

        public List<int> LedgerYears { get; set; } = new();
    }
}
