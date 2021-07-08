using System;
using System.Collections.Generic;
using DiffProject.Domain.AggregateModels.SeedWork;
using iffProject.Domain.AggregateModels.ComparisonAggregate;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate
{
    ///<summary>
    ///Entity that represents the data to be compared.
    ///</summary>
    public class BinaryData: Entity
    {
        public List<ComparisonSideEnum> ComparisonSide {get; private set;}

        public string Base64BinaryData {get; private set;}

        public BinaryData(List<ComparisonSideEnum> comparisonSide, string base64BinaryData) : base()
        {
            ComparisonSide = comparisonSide;
            Base64BinaryData = base64BinaryData;
            Validate(this,new BinaryDataValidator());
        }
    }
}
