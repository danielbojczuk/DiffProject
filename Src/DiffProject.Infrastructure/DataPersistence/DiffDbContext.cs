using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using Microsoft.EntityFrameworkCore;

namespace DiffProject.Infrastructure.DataPersistence
{
    /// <summary>
    /// Database context for the DiffProject.
    /// </summary>
    public class DiffDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiffDbContext"/> class.
        /// </summary>
        /// <param name="options">Instance of <see cref="DbContextOptions"/>.</param>
        public DiffDbContext(DbContextOptions<DiffDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the Binary Data <see cref="DbSet"/>.
        /// </summary>
        public DbSet<BinaryData> BinaryData { get; set; }

        /// <summary>
        /// Gets or sets the Comparison Results <see cref="DbSet"/>.
        /// </summary>
        public DbSet<ComparisonResult> ComparisonResults { get; set; }

        /// <inheritdoc cref="OnModelCreating"/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BinaryData>(
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
                });
        }
    }
}
