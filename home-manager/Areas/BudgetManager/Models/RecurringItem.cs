using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_manager.Areas.BudgetManager.Models
{
    [Table("tbl_recurring_item")]
    public class RecurringItem
    {
        [Column("rci_id")]
        public int Id { get; set; } = 0;

        [Column("rci_name")]
        public string Name { get; set; } = string.Empty;

        [Column("rci_description")]
        public string? Description { get; set; }

        [Column("rci_category_cat_id")]
        public int Category_catID { get; set; } = 10;

        [Column("rci_minimum_due")]
        public decimal MinimumDue { get; set; } = 0.0M;

        [Column("rci_balance")]
        public decimal? Balance { get; set; }

        [Column("rci_interest_rate")]
        public decimal? InterestRate { get; set; }

        [Column("rci_day")]
        public int Day { get; set; } = DateTime.Now.Day;

        [Column("rci_jan")]
        public bool Jan { get; set; } = false;

        [Column("rci_feb")]
        public bool Feb { get; set; } = false;

        [Column("rci_mar")]
        public bool Mar { get; set; } = false;

        [Column("rci_apr")]
        public bool Apr { get; set; } = false;

        [Column("rci_may")]
        public bool May { get; set; } = false;

        [Column("rci_jun")]
        public bool Jun { get; set; } = false;

        [Column("rci_jul")]
        public bool Jul { get; set; } = false;

        [Column("rci_aug")]
        public bool Aug { get; set; } = false;

        [Column("rci_sep")]
        public bool Sep { get; set; } = false;

        [Column("rci_oct")]
        public bool Oct { get; set; } = false;

        [Column("rci_nov")]
        public bool Nov { get; set; } = false;

        [Column("rci_dec")]
        public bool Dec { get; set; } = false;

        [Column("rci_paid_off")]
        public bool PaidOff { get; set; } = false;

        [Column("rci_date_created")]
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}