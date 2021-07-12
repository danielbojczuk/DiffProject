using DiffProject.Domain.AggregateModels.SeedWork;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate
{

    /// <summary>
    /// Difference Entity.
    /// It represents a difference between the Binary Data.
    /// </summary>
    public class Difference : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Difference"/> class.
        /// </summary>
        /// <param name="position">Start position.</param>
        /// <param name="size">Difference size.</param>
        public Difference(long position, long size)
        {
            Position = position;
            Size = size;
        }

        /// <summary>
        /// Gets the difference start position.
        /// </summary>
        public long Position { get; private set; }

        /// <summary>
        /// Gets the difference size (bytes).
        /// </summary>
        public long Size { get; private set; }

        /// <summary>
        /// Gets the parent Comparison Result.
        /// </summary>
        public ComparisonResult ComparisonResult { get; private set; }
    }
}
