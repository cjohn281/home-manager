using System.ComponentModel.DataAnnotations.Schema;

namespace home_manager.Areas.BudgetManager.Models
{
    [Table("tbl_recurring_savings_transfer_detail")]
    public class RecurringSavingsTransferDetail
    {
        [Column("rsd_id")]
        public int Id { get; set; } = 0;

        [Column("rsd_amount")]
        public decimal Amount { get; set; } = 0.0M;

        [Column("rsd_transfer_type_lvl_id")]
        public int TransferType_lvlId { get; set; } = 0;

        public string TransferTypeName { get; set; } = string.Empty;

        [Column("rsd_frequency_lvl_id")]
        public int Frequency_lvlId { get; set; } = 0;

        public string FrequencyDescription { get; set; } = string.Empty;

        [Column("rsd_start_date")]
        public DateTime? ValidDate { get; set; } = new DateTime(1970, 1, 1);

        [Column("rsd_extended_value1")]
        public int? Date1 { get; set; } = 0;

        [Column("rsd_extended_value2")]
        public int? Date2 { get; set; } = 0;
    }
}
