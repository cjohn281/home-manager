using System.ComponentModel.DataAnnotations.Schema;

namespace home_manager.Areas.BudgetManager.Models
{
    [Table("tbl_savings_ledger_item")]
    public class SavingsLedgerItem
    {
        [Column("sli_id")]
        public int Id { get; set; } = 0;

        [Column("sli_lookup_value_lvl_id")]
        public int Lookupvalue_lvlId { get; set; } = 23;

        [Column("sli_date")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Column("sli_amount")]
        public decimal Amount { get; set; } = 0.0M;

        public int Month { get; set; } = 0;

        public int Year { get; set; } = 0;
    }
}
