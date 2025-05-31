using Dapper;
using home_manager.Data;
using home_manager.Models;
using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace home_manager.Areas.BudgetManager.ViewModels
{
    public class Ledger_VModel
    {
        private readonly DbConnectionService _dbConnection;

        public Ledger_VModel(DbConnectionService dbConnection)
        {
            _dbConnection = dbConnection;
        }
    }

    public class AvailableLedgerDropdown_VModel
    {
        private readonly DbConnectionService _dbConnection;

        public AvailableLedgerDropdown_VModel(DbConnectionService dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public List<int> LedgerMonths { get; set; } = new();

        public List<int> LedgerYears { get; set; } = new();

        public async Task LoadAvailableLedgerDropdowns()
        {
            using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
            await connection.OpenAsync();
            var months = (await connection.QueryAsync<int>(
                "SELECT * FROM fnc_get_available_ledger_months()", new { }
            ))?.ToList();

            var years = (await connection.QueryAsync<int>(
                "SELECT * FROM fnc_get_available_ledger_years()", new { }
            ))?.ToList();

            if (months == null || months.Count == 0)
            {
                LedgerMonths = new List<int> { 0 };
            }
            else
            {
                LedgerMonths = months;
            }

            if (years == null || years.Count == 0)
            {
                LedgerYears = new List<int> { 0 };
            }
            else
            {
                LedgerYears = years;
            }
        }
    }
}
