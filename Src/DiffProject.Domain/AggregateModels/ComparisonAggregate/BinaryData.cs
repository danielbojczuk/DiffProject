using System;
using System.Collections.Generic;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Validators;
using DiffProject.Domain.AggregateModels.SeedWork;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate
{
    ///<summary>
    ///Entity that represents the data to be compared.
    ///</summary>
    public class BinaryData: Entity
    {
        /// <summary>
        /// Side de data should be on comparison
        /// </summary>
        public ComparisonSideEnum ComparisonSide {get; private set;}

        /// <summary>
        /// Base64 encoded binary Data
        /// </summary>
        public string Base64BinaryData {get; private set;}

        /// <summary>
        /// ComparisonId used in both sides and result.
        /// </summary>
        public Guid ComparisonId { get; private set; }


        public BinaryData(ComparisonSideEnum comparisonSide, string base64BinaryData, Guid comparisonId, IBinaryDataRepository binaryDataRepository) : base()
        {
            ComparisonSide = comparisonSide;
            Base64BinaryData = base64BinaryData;
            ComparisonId = comparisonId;
            Validate(this,new BinaryDataValidator());
            Validate(this, new BinaryDataDuplicityValidator(binaryDataRepository));
        }

        public void UpdateBase64BinaryFile(string base64BinaryFile)
        {
            Base64BinaryData = base64BinaryFile;
            Validate(this, new BinaryDataValidator());
        }
    }
}
