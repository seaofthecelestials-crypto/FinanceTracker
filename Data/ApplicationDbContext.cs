using ASP.NetLearning.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ASP.NetLearning.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Deposit> Deposits { get; set; }

        public override int SaveChanges()
        {
            NormalizeDateTimes();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            NormalizeDateTimes();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void NormalizeDateTimes()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                foreach (var prop in entry.Properties)
                {
                    if (prop.CurrentValue is DateTime dt && dt.Kind != DateTimeKind.Utc)
                    {
                        prop.CurrentValue = dt.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(dt, DateTimeKind.Utc) : dt.ToUniversalTime();

                    }
                }
            }
        }
    }
}