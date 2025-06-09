using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_manager.Areas.BudgetManager.Models
{
    [Table("tbl_transaction_type")]
    public class TransactionType
    {
        [Column("tst_id")]
        public int Id { get; set; } = 0;

        [Column("tst_description")]
        public string Description { get; set; } = String.Empty;
    }
}