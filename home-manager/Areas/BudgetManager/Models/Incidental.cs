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
    }
}