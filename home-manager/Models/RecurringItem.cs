using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_manager.Models
{
    [Table("tbl_recurring_item")]
    public class RecurringItem
    {
        [Key, Column("rci_id")]
        public int Id { get; set; }

        [Required, Column("rci_name")]
        public required string Name { get; set; }

        [Column("rci_description")]
        public string? Description { get; set; }

        [Column("rci_category_cat_id")]
        public int Category_catID { get; set; }

        [Required, Column("rci_minimum_due")]
        public required float MinimumDue { get; set; }

        [Column("rci_balance")]
        public float? Balance { get; set; }

        [Column("rci_interest_rate")]
        public float? InterestRate { get; set; }

        [Required, Column("rci_day")]
        public required int Day { get; set; }

        [Column("rci_jan")]
        public Boolean? Jan { get; set; }

        [Column("rci_feb")]
        public Boolean? Feb { get; set; }

        [Column("rci_mar")]
        public Boolean? Mar { get; set; }

        [Column("rci_apr")]
        public Boolean? Apr { get; set; }

        [Column("rci_may")]
        public Boolean? May { get; set; }

        [Column("rci_jun")]
        public Boolean? Jun { get; set; }

        [Column("rci_jul")]
        public Boolean? Jul { get; set; }

        [Column("rci_aug")]
        public Boolean? Aug { get; set; }

        [Column("rci_sep")]
        public Boolean? Sep { get; set; }

        [Column("rci_oct")]
        public Boolean? Oct { get; set; }

        [Column("rci_nov")]
        public Boolean? Nov { get; set; }

        [Column("rci_dec")]
        public Boolean? Dec { get; set; }

        [Column("rci_paid_off")]
        public Boolean? PaidOff { get; set; }

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