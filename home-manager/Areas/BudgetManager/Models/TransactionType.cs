using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_manager.Areas.BudgetManager.Models
{
    [Table("tbl_lookup_type")]
    public class TransactionType
    {
        [Column("lkt_id")]
        public int Id { get; set; } = 0;

        [Column("lkt_name")]
        public string Description { get; set; } = String.Empty;
    }
}