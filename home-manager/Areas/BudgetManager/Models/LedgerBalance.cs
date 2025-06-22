using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_manager.Areas.BudgetManager.Models
{
    [Table("tbl_ledger_balance")]
    public class LedgerBalance
    {
        [Key, Column("lbl_id")]
        public int Id { get; set; }

        [Required, Column("lbl_ledger_ldg_id")]
        public int Ledger_ldgID { get; set; }

        [Column("lbl_ending_checking_balance")]
        public float EndingCheckingBalance { get; set; }

        [Column("lbl_ending_savings_balance")]
        public float EndingSavingsBalance { get; set; }
    }
}
