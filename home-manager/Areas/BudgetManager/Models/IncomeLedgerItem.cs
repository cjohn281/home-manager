using System.ComponentModel.DataAnnotations.Schema;

namespace home_manager.Areas.BudgetManager.Models
{
    [Table("tbl_income_ledger_item")]
    public class IncomeLedgerItem
    {
        [Column("ili_id")]
        public int Id { get; set; } = 0;

        [Column("ili_person_prn_id")]
        public int Person_prnId { get; set; } = 0;

        public string PersonName { get; set; } = string.Empty;

        [Column("ili_date")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Column("ili_amount")]
        public decimal Amount { get; set; } = 0.0M;

        [Column("ili_paid")]
        public bool Paid { get; set; } = false;

        public int Month { get; set; } = 0;

        public int Year { get; set; } = 0;
    }
}
