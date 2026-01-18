using Microsoft.EntityFrameworkCore;
using SPCS.Common.Models;
using SPCS.Concurrency.Models;
using File = SPCS.Files.Models.File;
using SPCS.Files.Models;

namespace SPCS.Data
{
    public class SPCSContext : DbContext
    {
        public SPCSContext(DbContextOptions<SPCSContext> options) : base(options)
        {
        }

        public DbSet<ConcurrencyCalculation> ConcurrencyCalculations { get; set; }

        public DbSet<File> Files { get; set; }
        public DbSet<SPCS.Files.Models.Application> Applications { get; set; }
        public DbSet<SPCS.Files.Models.ParsedTimestamp> ParsedTimestamps { get; set; }

        public DbSet<SPCSConfiguration> Configuration { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Convert the enum to string in the DB
            modelBuilder.Entity<PowerTimestamp>().Property(p => p.Type).HasConversion<string>().IsRequired();

            // Configure PowerTimestamp decimal precision
            modelBuilder.Entity<PowerTimestamp>()
                .Property(p => p.Value)
                .HasPrecision(18, 10);

            // Configure the one-to-many relationship
            modelBuilder.Entity<PowerTimestamp>()
                .HasOne(p => p.ConcurrencyCalculation)
                .WithMany(c => c.PowerTimestamps)
                .HasForeignKey(p => p.ConcurrencyCalculationId)
                .IsRequired();

            // Precision for ConcurrencyCalculation decimal values
            modelBuilder.Entity<ConcurrencyCalculation>()
                .Property(c => c.ConcurrencyMetric)
                .HasPrecision(18, 10);

            modelBuilder.Entity<ConcurrencyCalculation>()
                .Property(c => c.NeedCoverage)
                .HasPrecision(18, 10);

            // Configure ParsedTimestamp decimal properties and relationships
            modelBuilder.Entity<SPCS.Files.Models.ParsedTimestamp>()
                .Property(p => p.ConsumptionValue)
                .HasPrecision(18, 10);

            modelBuilder.Entity<SPCS.Files.Models.ParsedTimestamp>()
                .Property(p => p.ProductionValue)
                .HasPrecision(18, 10);

            modelBuilder.Entity<SPCS.Files.Models.ParsedTimestamp>()
                .HasOne(p => p.File)
                .WithMany(f => f.ParsedTimestamps)
                .HasForeignKey(p => p.FileId)
                .IsRequired();

            // Configure File and Application relationship
            modelBuilder.Entity<SPCS.Files.Models.File>()
                .HasOne(f => f.Application)
                .WithMany(a => a.Files)
                .HasForeignKey(f => f.ApplicationId)
                .IsRequired(false);
        }



    }
}
