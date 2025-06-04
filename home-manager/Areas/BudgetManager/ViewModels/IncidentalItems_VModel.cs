using Dapper;
using home_manager.Areas.BudgetManager.Models;
using home_manager.Data;
using home_manager.Models;
using Npgsql;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static home_manager.Helpers.DropdownHelper;

namespace home_manager.Areas.BudgetManager.ViewModels
{
    public class IncidentalItems_VModel
    {

        public List<IncidentalItem> Items { get; set; } = new();
        public List<Category> DynamicCategoryOptions { get; set; } = new();
        public List<TransactionType> DynamicTransactionOptions { get; set; } = new();

        public decimal TotalAmount { get; private set; } = 0.0M;

    }
}
