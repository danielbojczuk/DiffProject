using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffProject.Infrastructure.DataPersistence
{
    public class DiffDbContext : DbContext
    {
        public DbSet<BinaryData> BinaryData { get; set; }
        public DiffDbContext(DbContextOptions<DiffDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BinaryData>(
                b =>
                {
                    b.HasKey(f => f.Id);
                    b.Ignore(f => f.ValidationResult);
                });
        }
    }
}
