using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace home_manager.Data
{
    public class DataProtectionContext : DbContext, IDataProtectionKeyContext
    {
        public DataProtectionContext(DbContextOptions<DataProtectionContext> options) : base(options) { }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataProtectionKey>().ToTable("tbl_data_protection_key");

            modelBuilder.Entity<DataProtectionKey>()
                .Property(d => d.Id)
                .HasColumnName("dpk_id");

            modelBuilder.Entity<DataProtectionKey>()
                .Property(d => d.FriendlyName)
                .HasColumnName("dpk_friendly_name");

            modelBuilder.Entity<DataProtectionKey>()
                .Property(d => d.Xml)
                .HasColumnName("dpk_xml_data");
        }

    }
}
