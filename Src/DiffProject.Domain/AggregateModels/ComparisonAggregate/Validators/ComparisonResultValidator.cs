using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using FluentValidation;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate.Validators
{
    /// <summary>
    /// The Validator of Bynary Data Entity.
    /// </summary>
    internal class ComparisonResultValidator : AbstractValidator<ComparisonResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComparisonResultValidator"/> class.
        /// It add the rules to be checked by Fluent Validator.
        /// </summary>
        /// <param name="validationToCompare">Value indicating if it should add the Comparing Validation Rule.</param>
        public ComparisonResultValidator(bool validationToCompare)
        {
            RuleFor(x => x.BinaryDataToCompare).Must(y => y.Count <= 2).WithMessage("You can't have more than two Binary Data to compare");
            RuleFor(x => x.BinaryDataToCompare).Must(y => y.FindAll(z => z.ComparisonSide == ComparisonSideEnum.Left).Count <= 1).WithMessage("You can't have more than two Bynary Data in the Left side");
            RuleFor(x => x.BinaryDataToCompare).Must(y => y.FindAll(z => z.ComparisonSide == ComparisonSideEnum.Right).Count <= 1).WithMessage("You can't have more than two Bynary Data in the Right side");

            // Comparing Validation Rule: It should be used only just before the comparing process.
            RuleFor(x => x.BinaryDataToCompare).Must((binaryDataToCompare) =>
            {
                if (!validationToCompare)
                {
                    return true;
                }

                return binaryDataToCompare.Count == 2;

            }).WithMessage("You need to have the both sides to compare");
        }
    }
}