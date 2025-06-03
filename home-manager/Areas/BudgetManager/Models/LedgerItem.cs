using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_manager.Areas.BudgetManager.Models
{
    [Table("tbl_ledger_item")]
    public class LedgerItem
    {
        [Key, Column("lgi_id")]
        public int Id { get; set; }

        [Required, Column("lgi_ledger_ldg_id")]
        public int Ledger_ldgID { get; set; }

        [Required, Column("lgi_is_recurring")]
        public bool IsRecurring { get; set; }

        [Column("lgi_recurring_item_rci_id")]
        public int RecurringItem_rciID { get; set; }

        [Column("lgi_incidental_item_ici_id")]
        public int IncidentalItem_iciID { get; set; }

        [Required, Column("lgi_amount")]
        public float Amount { get; set; }

        [Required, Column("lgi_is_paid")]
        public bool IsPaid { get; set; }

        [Column("lgi_date_created")]
        public DateTime DateCreated { get; set; }
    }
}