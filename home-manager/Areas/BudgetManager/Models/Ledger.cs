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

        [Column("ldg_date_created")]
        public DateTime DateCreated { get; set; }

        [Column("ldg_created_by_prn_id")]
        public int CreatedBy_prnId { get; set; }

        [Column("ldg_date_modified")]
        public DateTime DateModified { get; set; }

        [Column("ldg_modified_by_prn_id")]
        public int ModifiedBy_prnID { get; set; }
    }
}
