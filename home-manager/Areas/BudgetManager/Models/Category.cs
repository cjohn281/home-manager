using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_manager.Areas.BudgetManager.Models
{
    [Table("tbl_category")]
    public class Category
    {
        [Column("cat_id")]
        public int Id { get; set; } = 0;

        [Column("cat_transaction_type_tst_id")]
        public int TransactionType_tstID { get; set; } = 0;

        [Column("cat_description")]
        public string Description { get; set; } = String.Empty;

        [Column("cat_date_created")]
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}