using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using Microsoft.EntityFrameworkCore;

namespace DiffProject.Infrastructure.DataPersistence
{
    public class DiffDbContext : DbContext
    {
        public DbSet<BinaryData> BinaryData { get; set; }
        public DbSet<ComparisonResult> ComparisonResults { get; set; }
        public DiffDbContext(DbContextOptions<DiffDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<BinaryData>(
                b =>
                {
                    b.HasKey(f => f.Id);
                    b.Ignore(f => f.ValidationResult);
                });

            modelBuilder.Entity<ComparisonResult>(
                c =>
                {
                    c.HasKey(f => f.Id);
                    c.Ignore(f => f.ValidationResult);
                    c.HasMany(d => d.Differences).WithOne(r => r.ComparisonResult);
                });

            modelBuilder.Entity<Difference>(
                d =>
                {
                    d.HasKey(f => f.Id);
                    d.Ignore(f => f.ValidationResult);
                });*/
        }
    }
}
