using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using FluentValidation;


namespace DiffProject.Domain.AggregateModels.ComparisonAggregate.Validators
{
    /// <summary>
    /// Validations on DiffComparison Entity
    /// </summary>
    class DiffComparisonValidator: AbstractValidator<DiffComparison>
    {
        public DiffComparisonValidator()
        {
            //Duplicity validation for each Binary Data side 
            RuleFor(x => x.BinaryData).Must(y => y.FindAll(z => z.ComparisonSide == ComparisonSideEnum.Left).Count == 0).WithMessage("This Comparison already have Binary Data on the Left Side");
            RuleFor(x => x.BinaryData).Must(y => y.FindAll(z => z.ComparisonSide == ComparisonSideEnum.Right).Count == 0).WithMessage("This Comparison already have Binary Data on the Right Side");
        }
    }
}
