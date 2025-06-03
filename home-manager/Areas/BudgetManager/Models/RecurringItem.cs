using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_manager.Areas.BudgetManager.Models
{
    [Table("tbl_recurring_item")]
    public class RecurringItem
    {
        [Key, Column("rci_id")]
        public int Id { get; set; } = 0;

        [Required, Column("rci_name")]
        public required string Name { get; set; } = string.Empty;

        [Column("rci_description")]
        public string? Description { get; set; }

        [Required]
        [Column("rci_category_cat_id")]
        public int Category_catID { get; set; } = 10;

        [Required, Column("rci_minimum_due")]
        public required decimal MinimumDue { get; set; } = 0.0M;

        [Column("rci_balance")]
        public float? Balance { get; set; }

        [Column("rci_interest_rate")]
        public float? InterestRate { get; set; }

        [Required, Column("rci_day")]
        public required int Day { get; set; } = 1;

        [Column("rci_jan")]
        public bool? Jan { get; set; }

        [Column("rci_feb")]
        public bool? Feb { get; set; }

        [Column("rci_mar")]
        public bool? Mar { get; set; }

        [Column("rci_apr")]
        public bool? Apr { get; set; }

        [Column("rci_may")]
        public bool? May { get; set; }

        [Column("rci_jun")]
        public bool? Jun { get; set; }

        [Column("rci_jul")]
        public bool? Jul { get; set; }

        [Column("rci_aug")]
        public bool? Aug { get; set; }

        [Column("rci_sep")]
        public bool? Sep { get; set; }

        [Column("rci_oct")]
        public bool? Oct { get; set; }

        [Column("rci_nov")]
        public bool? Nov { get; set; }

        [Column("rci_dec")]
        public bool? Dec { get; set; }

        [Column("rci_paid_off")]
        public bool? PaidOff { get; set; }

        [Column("rci_date_created")]
        public DateTime DateCreated { get; set; }

        [Column("rci_created_by_prn_id")]
        public int CreatedBy_prnID { get; set; }

        [Column("rci_date_modified")]
        public DateTime DateModified { get; set; }

        [Column("rci_modified_by_prn_id")]
        public int ModifiedBy_prnID { get; set; }
    }
}