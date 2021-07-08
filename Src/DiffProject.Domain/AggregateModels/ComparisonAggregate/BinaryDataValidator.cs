using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using FluentValidation;

namespace iffProject.Domain.AggregateModels.ComparisonAggregate
{
    
    class BinaryDataValidator : AbstractValidator<BinaryData>
    {
        
        public BinaryDataValidator()
        {
            RuleFor(x => x.Base64BinaryData).NotNull().Must(x => CheckBase64(x)).WithMessage("Invalid Base64 String");
        }

        /// <summary>
        /// Validation of the Base64 string.
        /// </summary>
        /// <param name="base64BinaryData"></param>
        /// <returns>True if Base64 is valid it. Else it wil return False</returns>
        /// Usualy we do not use exceptions for validations, but in this case 
        /// validating if the string is base64 encoded is expensiver than handling the
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
