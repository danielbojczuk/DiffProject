using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiffProject.Infrastructure.DataPersistence
{
    ///<inheritdoc cref="IComparisonResultRepository"/>
    public class ComparisonResultRepository : IComparisonResultRepository
    {
        private readonly DiffDbContext _diffDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComparisonResultRepository"/> class.
        /// </summary>
        /// <param name="dbContext">Database Context<</param>
        public ComparisonResultRepository(DiffDbContext dbContext)
        {
            _diffDbContext = dbContext;
        }

        ///<inheritdoc cref="Add"/>
        public async Task<ComparisonResult> Add(ComparisonResult binaryData)
        {
            if (!binaryData.ValidationResult.IsValid)
                throw new InvalidOperationException("Invalid entity can not be persisted");

            List<ComparisonResult> listResult = await _diffDbContext.ComparisonResults.Where(x => x.ComparisonId == binaryData.ComparisonId).ToListAsync();
            _diffDbContext.ComparisonResults.RemoveRange(listResult);

            await _diffDbContext.ComparisonResults.AddAsync(binaryData);
            await _diffDbContext.SaveChangesAsync();
            return binaryData;
        }

        ///<inheritdoc cref="RetrieveResultByComparisonId"/>
        public async Task<ComparisonResult> RetrieveResultByComparisonId(Guid comparisonid)
        {
            return await _diffDbContext.ComparisonResults.Include(x => x.Differences).Where(x => x.ComparisonId == comparisonid).FirstOrDefaultAsync();
        }
    }
}
