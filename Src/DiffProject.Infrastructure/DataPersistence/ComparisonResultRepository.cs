using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiffProject.Infrastructure.DataPersistence
{
    public class ComparisonResultRepository : IComparisonResultRepository
    {
        public readonly DiffDbContext _diffDbContext;
        public ComparisonResultRepository(DiffDbContext dbContext)
        {
            _diffDbContext = dbContext;
        }
        public async Task<ComparisonResult> Add(ComparisonResult binaryData)
        {
            if (!binaryData.ValidationResult.IsValid)
                throw new InvalidOperationException("Invalid entity can not be persisted");
            await _diffDbContext.ComparisonResults.AddAsync(binaryData);
            await _diffDbContext.SaveChangesAsync();
            return binaryData;
        }

        public async Task<ComparisonResult> RetrieveResultByComparisonId(Guid comparisonid)
        {
            return await _diffDbContext.ComparisonResults.Include(x => x.Differences).Where(x => x.ComparisonId == comparisonid).FirstOrDefaultAsync();

        }
    }
}
