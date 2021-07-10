using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using FluentValidation;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate.Validators
{
    
    class BinaryDataDuplicityValidator : AbstractValidator<BinaryData>
    {

        /// <summary>
        /// Validations on BinaryData Entity duplicity
        /// </summary>
        public BinaryDataDuplicityValidator(IBinaryDataRepository binaryDataRepository)
        {
            RuleFor(x => x).Must(x => binaryDataRepository.RetrieveDBinaryDataByComparisonIdAndSide(x.ComparisonId,x.ComparisonSide) == null).WithMessage("There is already a Binary Data with this Comparison Id and Side");
        }

    }
}
