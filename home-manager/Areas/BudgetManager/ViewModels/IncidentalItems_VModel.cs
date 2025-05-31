using Dapper;
using home_manager.Data;
using home_manager.Models;
using Npgsql;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace home_manager.Areas.BudgetManager.ViewModels
{
    public class IncidentalItems_VModel
    {

        private readonly DbConnectionService _dbConnection;

        public List<IncidentalItem> Items { get; set; } = new();

        [Required]
        public decimal TotalAmount { get; private set; } = 0.0M;

        public IncidentalItems_VModel(DbConnectionService dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public class IncidentalItem
        {
            [Required]
            public int Id { get; set; } = -1;

            [Required]
            public int IncidentalId { get; set; } = -1;

            [Required]
            public string Name { get; set; } = String.Empty;

            public string Description { get; set; } = String.Empty;

            [Required]
            public DateTime DateIncurred { get; set; } = DateTime.Now;

            [Required]
            public decimal Amount { get; set; } = 0.0M;

            [Required]
            public int CategoryId { get; set; } = -1;

            [Required]
            public string CategoryName { get; set; } = String.Empty;
        }

        public async Task LoadIncidentalItemsAsync(int month, int year)
        {
            using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
            await connection.OpenAsync();
            var items = (await connection.QueryAsync<IncidentalItem>(
                "SELECT * FROM fnc_get_incidental_items_by_month(@month, @year)", new { month, year }
            ))?.ToList();

            if (items == null || items.Count == 0)
            {
                Items = new List<IncidentalItem> { new IncidentalItem() };
                TotalAmount = 0.0M;
            }
            else
            {
                Items = items;
                TotalAmount = Items.Sum(item => item.Amount);
            }
        }

    }
}
