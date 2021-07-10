using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using FluentValidation;
using System;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate.Validators
{

    class BinaryDataValidator : AbstractValidator<BinaryData>
    {

        IBinaryDataRepository _binaryDataRepository;

        /// <summary>
        /// Validations on BinaryData Entity 
        /// </summary>
        public BinaryDataValidator(IBinaryDataRepository binaryDataRepository, bool isUpdate)
        {
            _binaryDataRepository = binaryDataRepository;

            //Duplicity validation must be checked only for new Entities
            RuleFor(x => x).MustAsync(async (binaryData, cancellation) =>
            {
                if (isUpdate)
                    return true;

                BinaryData existingBinaryData = await _binaryDataRepository.RetrieveDBinaryDataByComparisonIdAndSide(binaryData.ComparisonId, binaryData.ComparisonSide);
                return existingBinaryData == null;

            }
            ).WithMessage("There is already a Binary Data with this Comparison Id and Side");

            //Base64 Validation
            RuleFor(x => x.Base64BinaryData).NotNull().Must(x => CheckBase64(x)).WithMessage("Invalid Base64 String");
        }

        /// <summary>
        /// Validation of the Base64 string.
        /// </summary>
        /// <param name="base64BinaryData"></param>
        /// <returns>True if Base64 is valid it. Else it wil return False</returns>
        /// Usualy we do not use exceptions for validations, but in this case 
        /// validating if the string is base64 encoded is more expensive than handling the
        /// exception.
        private bool CheckBase64(string base64BinaryData)
        {
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
