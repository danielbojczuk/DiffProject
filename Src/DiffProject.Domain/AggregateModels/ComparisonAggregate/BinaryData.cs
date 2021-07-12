using System;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Validators;
using DiffProject.Domain.AggregateModels.SeedWork;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate
{
    /// <summary>
    /// Binary Data Entity.
    /// It represents the binary data to be compared.
    /// </summary>
    public class BinaryData : Entity
    {
        private readonly IBinaryDataRepository _binaryDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryData"/> class.
        /// </summary>
        /// <param name="comparisonSide">Binary Data's side.</param>
        /// <param name="base64BinaryData">Base64 encoded string of the Binary Data to be compared.</param>
        /// <param name="comparisonId">Comparions Id.</param>
        /// <param name="binaryDataRepository">Instance of <see cref="IBinaryDataRepository"/> implementation. It will be used in the Dupliated Entity Validation Rule.</param>
        public BinaryData(ComparisonSideEnum comparisonSide, string base64BinaryData, Guid comparisonId, IBinaryDataRepository binaryDataRepository) 
            : base()
        {
            ComparisonSide = comparisonSide;
            Base64BinaryData = base64BinaryData;
            ComparisonId = comparisonId;
            _binaryDataRepository = binaryDataRepository;
            Validate(this, new BinaryDataValidator(_binaryDataRepository, false));
        }

        private BinaryData()
            : base()
        {
        }


        /// <summary>
        /// Gets the Binary Data's side.
        /// </summary>
        public ComparisonSideEnum ComparisonSide { get; private set; }

        /// <summary>
        /// Gets Bynary Data to be compared. It should be a valid Base64 encoded string.
        /// </summary>
        public string Base64BinaryData { get; private set; }

        /// <summary>
        /// Gets the Comparison Id. It should be the same for the both Binary Data to be compared.
        /// </summary>
        public Guid ComparisonId { get; private set; }

        /// <summary>
        /// Updates de Bynary Data to be compared.
        /// </summary>
        /// <param name="base64BinaryFile">Bynary Data to be compared. It should be a valid Base64 encoded string.</param>
        public void UpdateBase64BinaryFile(string base64BinaryFile)
        {
            Base64BinaryData = base64BinaryFile;
            Validate(this, new BinaryDataValidator(_binaryDataRepository, true));
        }
    }
}
