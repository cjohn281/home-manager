using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetManager.Models
{
    [Table("tbl_category")]
    public class Category
    {
        [Key, Column("cat_id")]
        public int Id { get; set; }

        [Required, Column("cat_transaction_type_tst_id")]
        public int TransactionType_tstID { get; set; }

        [Required, Column("cat_description")]
        public required string Description { get; set; }

        [Column("cat_date_created")]
        public DateTime DateCreated { get; set; }

        [Column("cat_created_by_prn_id")]
        public int CreatedBy_prnID { get; set; }

        [Column("cat_date_modified")]
        public DateTime DateModified { get; set; }

        [Column("cat_modified_by_prn_id")]
        public int ModifiedBy_prnID { get; set; }
    }
}
