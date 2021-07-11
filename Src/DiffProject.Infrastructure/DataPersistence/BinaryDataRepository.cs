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
            return await (from b in _diffDbContext.BinaryData
                         where b.Active == true && b.ComparisonId == comparisonid
                         select b).ToListAsync();
        }

        public async Task<BinaryData> RetrieveDBinaryDataByComparisonIdAndSide(Guid comparisonid, ComparisonSideEnum side)
        {
            return await _diffDbContext.BinaryData.FirstOrDefaultAsync(x => x.Active == true && x.ComparisonId == comparisonid && x.ComparisonSide == side);
        }

        public async Task<BinaryData> Update(BinaryData binaryData)
        {
            if (!binaryData.ValidationResult.IsValid)
                throw new InvalidOperationException("Invalid entity can not be persisted");

            BinaryData oldBinaryId = await RetrieveDBinaryDataById(binaryData.Id);
            oldBinaryId.DesactivateEntity();
            _diffDbContext.BinaryData.Update(oldBinaryId);

            BinaryData newBinaryData = new BinaryData(binaryData.ComparisonSide, binaryData.Base64BinaryData, binaryData.ComparisonId, this);
            await _diffDbContext.BinaryData.AddAsync(newBinaryData);

            await _diffDbContext.SaveChangesAsync();
            return newBinaryData;
        }

        public async Task<BinaryData> RetrieveDBinaryDataById(Guid binaryDataId)
        {
            return await _diffDbContext.BinaryData.FirstOrDefaultAsync(x => x.Id == binaryDataId);
        }
    }
}
