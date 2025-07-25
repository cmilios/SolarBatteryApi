using Microsoft.EntityFrameworkCore;
using SPCS.Common.Models;
using SPCS.Concurrency.Models;
using File = SPCS.Files.Models.File;

namespace SPCS.Data
{
    public class SPCSContext : DbContext
    {
        public SPCSContext(DbContextOptions<SPCSContext> options) : base(options)
        {
        }

        public DbSet<ConcurrencyCalculation> ConcurrencyCalculations { get; set; }

        public DbSet<File> Files { get; set; }

        public DbSet<SPCSConfiguration> Configuration { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Convert the enum to string in the DB
            modelBuilder.Entity<PowerTimestamp>()
                .Property(p => p.Type)
                .HasPrecision(18, 6)
                .HasConversion<string>()
                .IsRequired();

            // Configure the one-to-many relationship
            modelBuilder.Entity<PowerTimestamp>()
                .HasOne(p => p.ConcurrencyCalculation)
                .WithMany(c => c.PowerTimestamps)
                .HasForeignKey(p => p.ConcurrencyCalculationId)
                .IsRequired();

            // Optional: Precision for decimal values
            modelBuilder.Entity<ConcurrencyCalculation>()
                .Property(c => c.ConcurrencyMetric)
                .HasPrecision(18, 10); // adjust as needed

            modelBuilder.Entity<ConcurrencyCalculation>()
                .Property(c => c.NeedCoverage)
                .HasPrecision(18, 10); // adjust as needed
        }



    }
}
