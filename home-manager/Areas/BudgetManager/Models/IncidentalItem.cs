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
        public int Incidental_incID { get; set; } = 0;

        [Column("ici_name")]
        public string Name { get; set; } = String.Empty;

        [Column("ici_description")]
        public string Description { get; set; } = String.Empty;

        [Column("ici_date")]
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [Column("ici_amount")]
        public decimal Amount { get; set; } = 0.0M;

        [Column("ici_category_cat_id")]
        public int Category_catID { get; set; } = 0;
    }
}