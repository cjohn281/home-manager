using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace home_manager.Areas.BudgetManager.ViewModels
{
    public class RecurringItems_VModel
    {
        private readonly CultureInfo _cultureInfo = new("en-US");

        public string FormatCurrency(decimal amount)
        {
            return amount.ToString("C", _cultureInfo);
        }

        public List<RecurringItem> Items { get; set; } = new();

        [Required]
        public decimal TotalMinimumDue { get; private set; } = 0.0M;

        [Required]
        public decimal TotalBalance { get; private set; } = 0.0M;

        public class RecurringItem
        {
            [Required]
            public int Id { get; set; }

            [Required]
            public string Name { get; set; } = String.Empty;

            public string? Description { get; set; }

            [Required]
            public int CategoryId { get; set; }

            [Required]
            public string CategoryName { get; set; } = String.Empty;

            [Required]
            public decimal MinimumDue { get; set; }

            public decimal? Balance { get; set; }

            public decimal? InterestRate { get; set; }

            [Required]
            public int DayOfMonth { get; set; }

            [Required]
            public bool January { get; set; } = false;

            [Required]
            public bool February { get; set; } = false;

            [Required]
            public bool March { get; set; } = false;

            [Required]
            public bool April { get; set; } = false;

            [Required]
            public bool May { get; set; } = false;

            [Required]
            public bool June { get; set; } = false;

            [Required]
            public bool July { get; set; } = false;

            [Required]
            public bool August { get; set; } = false;

            [Required]
            public bool September { get; set; } = false;

            [Required]
            public bool October { get; set; } = false;

            [Required]
            public bool November { get; set; } = false;

            [Required]
            public bool December { get; set; } = false;

            [Required]
            public bool PaidOff { get; set; } = false;
        }

        public async Task LoadItemsAsync(string connectionString)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync(); // Ensure the connection is opened
            Items = (await connection.QueryAsync<RecurringItem>(
                "SELECT * FROM fnc_get_recurring_items();"
            )).ToList();

            TotalMinimumDue = Items.Sum(item => item.MinimumDue);

            TotalBalance = Items.Sum(item => item.Balance ?? 0);
        }
    }
}
