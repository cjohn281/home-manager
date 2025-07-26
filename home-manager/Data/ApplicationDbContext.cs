using Microsoft.EntityFrameworkCore;
using home_manager.Areas.BudgetManager.Models;
using home_manager.Models;

namespace home_manager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Incidental> Incidentals { get; set; }
        public DbSet<IncidentalItem> IncidentalItems { get; set; }
        public DbSet<Ledger> Ledgers { get; set; }
        public DbSet<LedgerBalance> LedgerBalances { get; set; }
        public DbSet<LedgerItem> LedgerItems { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<RecurringItem> RecurringItems { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
    }
}
