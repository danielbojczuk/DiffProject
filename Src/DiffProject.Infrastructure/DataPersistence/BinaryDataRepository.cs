using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DiffProject.Infrastructure.DataPersistence
{
    ///<inheritdoc cref="IBinaryDataRepository"/>
    public class BinaryDataRepository : IBinaryDataRepository
    {
        private readonly DiffDbContext _diffDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryDataRepository"/> class.
        /// </summary>
        /// <param name="dbContext">Database Context</param>
        public BinaryDataRepository(DiffDbContext dbContext)
        {
            _diffDbContext = dbContext;
        }

        /// <inheritdoc cref="Add"/>
        public async Task<BinaryData> Add(BinaryData binaryData)
        {
            if (!binaryData.ValidationResult.IsValid)
            {
                throw new InvalidOperationException("Invalid entity can not be persisted");
            }

            await _diffDbContext.BinaryData.AddAsync(binaryData);
            await _diffDbContext.SaveChangesAsync();
            return binaryData;
        }

        /// <inheritdoc cref="RetrieveDBinaryDataByComparisonId"/>
        public async Task<List<BinaryData>> RetrieveDBinaryDataByComparisonId(Guid comparisonid)
        {
            return await _diffDbContext.BinaryData.Where(b => b.ComparisonId == comparisonid).ToListAsync();
        }

        /// <inheritdoc cref="RetrieveDBinaryDataByComparisonIdAndSide"/>
        public async Task<BinaryData> RetrieveDBinaryDataByComparisonIdAndSide(Guid comparisonid, ComparisonSideEnum side)
        {
            return await _diffDbContext.BinaryData.FirstOrDefaultAsync(x => x.ComparisonId == comparisonid && x.ComparisonSide == side);
        }

        /// <inheritdoc cref="Update"/>
        public async Task<BinaryData> Update(BinaryData binaryData)
        {
            if (!binaryData.ValidationResult.IsValid)
            {
                throw new InvalidOperationException("Invalid entity can not be persisted");
            }

            _diffDbContext.BinaryData.Update(binaryData);

            await _diffDbContext.SaveChangesAsync();
            return binaryData;
        }
    }
}
