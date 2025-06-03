using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_manager.Areas.BudgetManager.Models
{
    [Table("tbl_incidental")]
    public class Incidental
    {
        [Key, Column("inc_id")]
        public int Id { get; set; }

        [Column("inc_ledger_ldg_id")]
        public int Ledger_ldgID { get; set; }

        [Column("inc_date_created")]
        public DateTime DateCreated { get; set; }

        [Column("inc_created_by_prn_id")]
        public int CreatedBy_prnId { get; set; }

        [Column("inc_date_modified")]
        public DateTime DateModified { get; set; }

        [Column("inc_modified_by_prn_id")]
        public int ModifiedBy_prnID { get; set; }
    }
}