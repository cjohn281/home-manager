using System.ComponentModel.DataAnnotations.Schema;

namespace home_manager.Areas.BudgetManager.Models
{
    [Table("tbl_income_detail")]
    public class IncomeDetail
    {
        [Column("icd_id")]
        public int Id { get; set; } = 0;

        [Column("icd_person_prn_id")]
        public int Person_prnId { get; set; } = 0;

        public string PersonName { get; set; } = string.Empty;

        [Column("icd_default_amount")]
        public decimal DefaultAmount { get; set; } = 0.0M;

        [Column("icd_lookup_value_lvl_id")]
        public int LookupValue_lvlId { get; set; } = 0;

        public string PayFrequency { get; set; } = string.Empty;

        [Column("icd_extended_value1")]
        public int? PayDate1 { get; set; }

        [Column("icd_extended_value2")]
        public int? PayDate2 { get; set; }

        [Column("icd_first_pay")]
        public DateTime? ValidDate { get; set; }
    }
}
