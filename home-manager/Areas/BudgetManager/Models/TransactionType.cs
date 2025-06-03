using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_manager.Areas.BudgetManager.Models
{
    [Table("tbl_transaction_type")]
    public class TransactionType
    {
        [Key, Column("tst_id")]
        public int Id { get; set; }

        [Required, Column("tst_description")]
        public required string Description { get; set; }

        [Column("tst_date_created")]
        public DateTime DateCreated { get; set; }

        [Column("tst_created_by_prn_id")]
        public int CreatedBy_prnID { get; set; }

        [Column("tst_date_modified")]
        public DateTime DateModified { get; set; }

        [Column("tst_modified_by_prn_id")]
        public int ModifiedBy_prnID { get; set; }
    }
}