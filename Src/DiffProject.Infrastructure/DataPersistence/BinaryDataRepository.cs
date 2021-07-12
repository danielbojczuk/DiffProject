using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiffProject.Infrastructure.DataPersistence
{
    public class BinaryDataRepository : IBinaryDataRepository
    {
        public readonly DiffDbContext _diffDbContext;

        public BinaryDataRepository(DiffDbContext dbContext)
        {
            _diffDbContext = dbContext;
        }

        public async Task<BinaryData> Add(BinaryData binaryData)
        {
            if (!binaryData.ValidationResult.IsValid)
                throw new InvalidOperationException("Invalid entity can not be persisted");

            await _diffDbContext.BinaryData.AddAsync(binaryData);
            await _diffDbContext.SaveChangesAsync();
            return binaryData;
        }

        public async Task<List<BinaryData>> RetrieveDBinaryDataByComparisonId(Guid comparisonid)
        {
            return await _diffDbContext.BinaryData.Where(b => b.ComparisonId == comparisonid).ToListAsync();
        }

        public async Task<BinaryData> RetrieveDBinaryDataByComparisonIdAndSide(Guid comparisonid, ComparisonSideEnum side)
        {
            return await _diffDbContext.BinaryData.FirstOrDefaultAsync(x => x.ComparisonId == comparisonid && x.ComparisonSide == side);
        }

        public async Task<BinaryData> Update(BinaryData binaryData)
        {            
            if (!binaryData.ValidationResult.IsValid)
                throw new InvalidOperationException("Invalid entity can not be persisted");

            _diffDbContext.BinaryData.Update(binaryData);

            await _diffDbContext.SaveChangesAsync();
            return binaryData;
        }
    }
}
