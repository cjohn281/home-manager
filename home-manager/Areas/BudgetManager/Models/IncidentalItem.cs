using home_manager.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_manager.Areas.BudgetManager.Models
{
    [Table("tbl_incidental_item")]
    public class IncidentalItem
    {
        [Column("ici_id")]
        public int Id { get; set; } = 0;

        [Column("ici_incidental_inc_id")]
        public int Incidental_incId { get; set; } = 0;

        [Column("ici_name")]
        public string Name { get; set; } = String.Empty;

        [Column("ici_description")]
        public string Description { get; set; } = String.Empty;

        [Column("ici_date")]
        public DateTime Date { get; set; } = TimeZoneHelper.LocalTime;

        [Column("ici_amount")]
        public decimal Amount { get; set; } = 0.0M;

        [Column("ici_is_paid")]
        public bool IsPaid { get; set; } = true;

        [Column("ici_lookup_value_lvl_id")]
        public int Category_catID { get; set; } = 0;

        public string CategoryName { get; set; } = String.Empty;

        public int TransactionType_tstId { get; set; } = 0;

        public string TransactionTypeName { get; set; } = String.Empty;

        public List<Category> CategoryList { get; set; } = new List<Category>();
    }
}

    