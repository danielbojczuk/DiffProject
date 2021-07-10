using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using FluentValidation;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate.Validators
{
    
    class BinaryDataValidator : AbstractValidator<BinaryData>
    {

        /// <summary>
        /// Validations on BinaryData Entity
        /// </summary>
        public BinaryDataValidator(IBinaryDataRepository binaryDataRepository)
        {
            RuleFor(x => x.Base64BinaryData).NotNull().Must(x => CheckBase64(x)).WithMessage("Invalid Base64 String");
            RuleFor(x => x).Must(x => binaryDataRepository.RetrieveDBinaryDataByComparisonIdAndSide(x.ComparisonId,x.ComparisonSide) == null).WithMessage("There is already a Binary Data with this Comparison Id and Side");
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
