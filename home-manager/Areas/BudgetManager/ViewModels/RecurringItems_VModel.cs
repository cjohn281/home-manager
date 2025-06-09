using Dapper;
using home_manager.Areas.BudgetManager.Models;
using home_manager.Areas.BudgetManager.Repositories;
using home_manager.Data;
using home_manager.Models;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace home_manager.Areas.BudgetManager.ViewModels
{
    /// <summary>
    /// View model for managing a collection of recurring expense items.
    /// Provides functionality to load and calculate totals for recurring items.
    /// </summary>
    public class RecurringItems_VModel
    {
        public List<RecurringItem> Items { get; set; } = new();

        public decimal TotalMinimumDue { get; private set; } = 0.0M;

        public decimal TotalBalance { get; private set; } = 0.0M;

        public Dictionary<int, string> CategoryNames { get; set; } = new();

        public void CalculateTotals()
        {
            TotalMinimumDue = Items.Sum(item => item.MinimumDue);
            TotalBalance = Items.Sum(item => item.Balance ?? 0);
        }

        public string GetCategoryName(RecurringItem item)
        {
            if (item == null) return "N/A";
            return CategoryNames.TryGetValue(item.Category_catId, out var name) ? name : "N/A";
        }

    }

    /// <summary>
    /// View model for managing recurring expense categories used in filters.
    /// </summary>
    public class RecurringCategoryFilterItems_VModel
    {
        public List<Category> Categories { get; set; } = new();
    }

    /// <summary>
    /// View model for modifying a single recurring expense item.
    /// </summary>
    public class ModifyItem_VModel
    {
        public RecurringItem Item { get; set; } = new();

    }


    // Update the initialization of ModifyItem property in ModifyItemCombinedViewModel to pass the required parameter.

    public class ModifyItemCombinedViewModel
    {
        // Fix for CS8618: Initialize the property in the constructor to ensure it is non-nullable.  
        public ModifyItemCombinedViewModel()
        {
            ModifyItem = new ModifyItem_VModel();
            Categories = new RecurringCategoryFilterItems_VModel();
        }

        public ModifyItem_VModel ModifyItem { get; set; }

        public RecurringCategoryFilterItems_VModel Categories { get; set; }
    }
}
