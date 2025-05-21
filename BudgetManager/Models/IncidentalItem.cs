using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetManager.Models
{
    [Table("tbl_incidental_item")]
    public class IncidentalItem
    {
        [Key, Column("ici_id")]
        public int Id { get; set; }

        [Column("ici_incidental_inc_id")]
        public int Incidental_incID { get; set; }

        [Required, Column("ici_name")]
        public required string Name { get; set; }

        [Column("ici_description")]
        public string? Description { get; set; }

        [Required, Column("ici_date")]
        public required DateOnly Date {  get; set; }

        [Required, Column("ici_amount")]
        public float Amount { get; set; }

        [Required, Column("ici_category_cat_id")]
        public required int Category_catID { get; set; }

        [Column("ici_date_created")]
        public DateTime DateCreated { get; set; }

        [Column("ici_created_by_prn_id")]
        public int CreatedBy_prnID { get; set; }

        [Column("ici_date_modified")]
        public DateTime DateModified { get; set; }

        [Column("ici_modified_by_prn_id")]
        public int ModifiedBy_prnID { get; set; }
    }
}
