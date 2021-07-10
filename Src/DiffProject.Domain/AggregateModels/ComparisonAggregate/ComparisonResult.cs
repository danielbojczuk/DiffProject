using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Validators;
using DiffProject.Domain.AggregateModels.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate
{
    ///<summary>
    ///Entity that represents the data to be compared.
    ///</summary>
    public class ComparisonResult : Entity
    {
        /// <summary>
        /// It will be true only if both sides are totaly equal. It will be null if it has not been calculated yet.
        /// </summary>
        public bool? SidesEqual
        {
            get
            {
                if (SameSize == null)
                    return null;

                return (bool)SameSize && Differences.Count == 0;
            }
        }

        /// <summary>
        /// It will be true only if both sides are totaly equal. It will be null if it has not been calculated yet.
        /// </summary>
        public bool? SameSize { get; private set; }

        /// <summary>
        /// It will have the differences between the sides if they have the same size.
        /// </summary>
        /// <typeparam name="long">The fist type parameter is the offset</typeparam>
        /// <typeparam name="long">The second type parameter is the lenght</typeparam>
        public Dictionary<long, long> Differences { get; private set; }

        /// <summary>
        /// ComparisonId used in both sides and result.
        /// </summary>
        public Guid ComparisonId { get; private set; }

        /// <summary>
        /// The Binary Data to be compared.
        /// </summary>
        public List<BinaryData> BinaryDataToCompare { get; private set; }


        public ComparisonResult(Guid comparisonId) : base()
        {
            SameSize = null;
            Differences = new Dictionary<long, long>();
            BinaryDataToCompare = new List<BinaryData>();
            ComparisonId = comparisonId;
        }

        /// <summary>
        /// Add the BinaryData to be used in the difference calculation
        /// </summary>
        /// <param name="binaryDataList"></param>
        public void AddDataToCompare(List<BinaryData> binaryDataList)
        {
            BinaryDataToCompare.AddRange(binaryDataList);
            Validate(this, new ComparisonResultValidator(false));
        }

        /// <summary>
        /// Compare the bynary data using the logic
        /// 
        /// If both sides have the same lenght it will iterate throu the byte array to check if they are
        /// equal. If some diferece were found it will add the position and lengh of the difference.
        /// </summary>
        public void Compare()
        {
            Validate(this, new ComparisonResultValidator(true));
            if(ValidationResult.IsValid == true)
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
                        } else if (leftSide[i] == rightSide[i] && sequenceEqual == false)
                        {
                            Differences.Add(offset, i - offset);
                            sequenceEqual = true;
                        }
                    }
                    if(sequenceEqual == false)
                        Differences.Add(offset, leftSide.Length - offset);
                }
            }
        }

        /// <summary>
        /// Update the the SameSize result
        /// </summary>
        /// <param name="lefSideLenght"></param>
        /// <param name="rightSideLenght"></param>
        private void UpdateSamesize(int lefSideLenght, int rightSideLenght)
        {
            if (lefSideLenght == rightSideLenght)
                SameSize = true;
            else
                SameSize = false;
        }

    }
}
