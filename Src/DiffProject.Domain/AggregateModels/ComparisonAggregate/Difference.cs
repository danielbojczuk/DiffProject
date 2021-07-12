using DiffProject.Domain.AggregateModels.SeedWork;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate
{
    public class Difference : Entity
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
