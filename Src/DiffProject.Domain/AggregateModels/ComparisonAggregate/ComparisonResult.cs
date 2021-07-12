using System;
using System.Collections.Generic;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Validators;
using DiffProject.Domain.AggregateModels.SeedWork;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate
{
    /// <summary>
    /// Comparison Result Entity.
    /// It represents differences calculation result.
    /// </summary>
    public class ComparisonResult : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComparisonResult"/> class.
        /// </summary>
        /// <param name="comparisonId">Comparison Id used in both Binary Data.</param>
        public ComparisonResult(Guid comparisonId)
            : base()
        {
            SameSize = null;
            Differences = new List<Difference>();
            BinaryDataToCompare = new List<BinaryData>();
            ComparisonId = comparisonId;
        }

        private ComparisonResult()
        {
        }

        /// <summary>
        /// Gets the value indicating whether the both comparison sides are equal or not..
        /// </summary>
        public bool? SidesEqual
        {
            get
            {
                if (SameSize == null)
                {
                    return null;
                }

                return (bool)SameSize && Differences.Count == 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the both comparison sides have the same sizes or not.
        /// </summary>
        public bool? SameSize { get; private set; }

        /// <summary>
        /// Gets  the differences between the sides. It should be set only if both sides have the same sizes.
        /// </summary>
        public List<Difference> Differences { get; private set; }

        /// <summary>
        /// Gets the Comparison ID..
        /// </summary>
        public Guid ComparisonId { get; private set; }

        /// <summary>
        /// Gets a list with the Binary Data to be compared.
        /// </summary>
        public List<BinaryData> BinaryDataToCompare { get; private set; }

        /// <summary>
        /// Add the BinaryData to be used in the differences calculation.
        /// </summary>
        /// <param name="binaryDataList">A list with the Binary Data to be compared.</param>
        public void AddDataToCompare(List<BinaryData> binaryDataList)
        {
            BinaryDataToCompare.AddRange(binaryDataList);
            Validate(this, new ComparisonResultValidator(false));
        }

        /// <summary>
        /// Compare the provided Binary Data.
        /// It will only get the differences if the Binary Data have the same size.
        /// </summary>
        public void Compare()
        {
            Validate(this, new ComparisonResultValidator(true));
            if (ValidationResult.IsValid == true)
            {
                byte[] leftSide = Convert.FromBase64String(BinaryDataToCompare.Find(x => x.ComparisonSide == ComparisonSideEnum.Left).Base64BinaryData);
                byte[] rightSide = Convert.FromBase64String(BinaryDataToCompare.Find(x => x.ComparisonSide == ComparisonSideEnum.Right).Base64BinaryData);

                UpdateSamesize(leftSide.Length, rightSide.Length);

                if (SameSize == true)
                {
                    bool sequenceEqual = true;
                    int offset = 0;
                    for (int i = 0; i < leftSide.Length; i++)
                    {
                        if (leftSide[i] != rightSide[i] && sequenceEqual == true)
                        {
                            offset = i;
                            sequenceEqual = false;
                        }
                        else if (leftSide[i] == rightSide[i] && sequenceEqual == false)
                        {
                            Differences.Add(new Difference(offset, i - offset));
                            sequenceEqual = true;
                        }
                    }
                    if (sequenceEqual == false)
                    {
                        Differences.Add(new Difference(offset, leftSide.Length - offset));
                    }
                }
            }
        }

        /// <summary>
        /// Check if the Bynary Data have the same size and update the the SameSize property.
        /// </summary>
        /// <param name="lefSideLenght">Lenght of the Binary Data on the left size.</param>
        /// <param name="rightSideLenght">Lenght of the Binary Data on the right size.</param>
        private void UpdateSamesize(int lefSideLenght, int rightSideLenght)
        {
            if (lefSideLenght == rightSideLenght)
            {
                SameSize = true;
            }
            else
            {
                SameSize = false;
            }
        }
    }
}
