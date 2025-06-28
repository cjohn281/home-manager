using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_manager.Areas.BudgetManager.Models
{
    [Table("tbl_lookup_value")]
    public class Category
    {
        [Column("lvl_id")]
        public int Id { get; set; } = 0;

        [Column("lvl_lookup_type_lkt_id")]
        public int TransactionType_tstId { get; set; } = 0;

        [Column("lvl_text")]
        public string Description { get; set; } = String.Empty;
    }
}