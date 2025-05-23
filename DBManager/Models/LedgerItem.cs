﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBManager.Models
{
    [Table("tbl_ledger_item")]
    public class LedgerItem
    {
        [Key, Column("lgi_id")]
        public int Id { get; set; }

        [Required, Column("lgi_ledger_ldg_id")]
        public int Ledger_ldgID { get; set; }

        [Required, Column("lgi_is_recurring")]
        public Boolean IsRecurring { get; set; }

        [Column("lgi_recurring_item_rci_id")]
        public int RecurringItem_rciID { get; set; }

        [Column("lgi_incidental_item_ici_id")]
        public int IncidentalItem_iciID { get; set; }

        [Required, Column("lgi_amount")]
        public float Amount { get; set; }

        [Required, Column("lgi_is_paid")]
        public Boolean IsPaid { get; set; }

        [Column("lgi_date_created")]
        public DateTime DateCreated { get; set; }

        [Column("lgi_created_by_prn_id")]
        public int CreatedBy_prnID { get; set; }

        [Column("lgi_date_modified")]
        public DateTime DateModified { get; set; }

        [Column("lgi_modified_by_prn_id")]
        public int ModifiedBy_prnID { get; set; }
    }
}
