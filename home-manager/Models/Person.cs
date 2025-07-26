using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_manager.Models
{
    [Table("tbl_person")]
    public class Person
    {
        [Key, Column("prn_id")]
        public int Id { get; set; }

        [Required, Column("prn_name")]
        public required string Name { get; set; }

        [Required, Column("prn_email")]
        public required string Email { get; set; }

        [Required, Column("prn_salt")]
        public required string Salt { get; set; }

        [Required, Column("prn_password")]
        public required string Password { get; set; }

        [Column("prn_date_created")]
        public DateTime DateCreated { get; set; }

        [Column("prn_role_lvl_id")]
        public required int Role { get; set; } = 39;
    }
}