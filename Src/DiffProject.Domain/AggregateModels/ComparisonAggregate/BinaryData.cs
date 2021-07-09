using System;
using System.Collections.Generic;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Validators;
using DiffProject.Domain.AggregateModels.SeedWork;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate
{
    ///<summary>
    ///Entity that represents the data to be compared.
    ///</summary>
    public class BinaryData: Entity
    {
        public ComparisonSideEnum ComparisonSide {get; private set;}

        public string Base64BinaryData {get; private set;}

        public BinaryData(ComparisonSideEnum comparisonSide, string base64BinaryData) : base()
        {
            ComparisonSide = comparisonSide;
            Base64BinaryData = base64BinaryData;
            Validate(this,new BinaryDataValidator());
        }
    }
}
