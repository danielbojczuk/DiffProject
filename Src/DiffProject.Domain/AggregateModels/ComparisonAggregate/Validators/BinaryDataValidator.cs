using System;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using FluentValidation;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate.Validators
{
    /// <summary>
    /// The Validator of Bynary Data Entity.
    /// </summary>
    internal class BinaryDataValidator : AbstractValidator<BinaryData>
    {

        private readonly IBinaryDataRepository _binaryDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryDataValidator"/> class.
        /// It add the rules to be checked by Fluent Validator.
        /// </summary>
        /// <param name="binaryDataRepository">Instance of <see cref="IBinaryDataRepository"/> implementation</param>
        /// <param name="isUpdate">Value indicating if this is an entity update (<see cref="true"/>) or an entity creation (<see cref="false"/>).</param>
        public BinaryDataValidator(IBinaryDataRepository binaryDataRepository, bool isUpdate)
        {
            _binaryDataRepository = binaryDataRepository;

            // Duplicity validation. It must be checked only for new Entities
            RuleFor(x => x).MustAsync(async (binaryData, cancellation) =>
            {
                if (isUpdate)
                {
                    return true;
                }

                BinaryData existingBinaryData = await _binaryDataRepository.RetrieveDBinaryDataByComparisonIdAndSide(binaryData.ComparisonId, binaryData.ComparisonSide);
                return existingBinaryData == null;
            }).WithMessage("There is already a Binary Data with this Comparison Id and Side");

            // Base64 Validation
            RuleFor(x => x.Base64BinaryData).NotNull().Must(x => CheckBase64(x)).WithMessage("Invalid Base64 String");
        }

        /// <summary>
        /// Validation if the provided  Base64 encoded string is valid.
        /// </summary>
        /// <param name="base64BinaryData">Base64 encoded string to validate.</param>
        /// <returns><see cref="true"/> if the Base64 encoded string is valid , otherwise <see cref="false"/>.</returns>
        private static bool CheckBase64(string base64BinaryData)
        {
            // Usualy we should not use exceptions for validations,
            // but checking if a Base64 encoded string is valid
            // could be more expensive than handling an exception.
            try
            {
                byte[] binaryData;
                binaryData = Convert.FromBase64String(base64BinaryData);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
