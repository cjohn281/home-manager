using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_manager.Areas.BudgetManager.Models
{
    [Table("tbl_ledger")]
    public class Ledger
    {
        [Key, Column("ldg_id")]
        public int Id { get; set; }

        [Required, Column("ldg_month")]
        public required int Month { get; set; }

        [Required, Column("ldg_year")]
        public required int Year { get; set; }
    }
}
