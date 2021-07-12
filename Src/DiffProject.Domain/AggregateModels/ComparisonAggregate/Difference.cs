using DiffProject.Domain.AggregateModels.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate
{
    public class Difference:Entity
    {
        public long Position { get; private set; }
        public long Size { get; private set; }

        public ComparisonResult ComparisonResult { get; private set; }

        public Difference(long position, long size)
        {
            Position = position;
            Size = size;
        }
    }
}
