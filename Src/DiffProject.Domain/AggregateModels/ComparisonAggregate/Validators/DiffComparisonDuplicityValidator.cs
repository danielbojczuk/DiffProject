using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using FluentValidation;


namespace DiffProject.Domain.AggregateModels.ComparisonAggregate.Validators
{
    /// <summary>
    /// Validator to check if there is already a validation with the given id
    /// </summary>
    class DiffComparisonDuplicityValidator : AbstractValidator<DiffComparison>
    {
        public DiffComparisonDuplicityValidator(IDiffComparisonRepository diffComparisonRepository)
        {
            RuleFor(x => x.Id).Must(y => diffComparisonRepository.RetrieveDiffComparisonById(y) == null).WithMessage("Ther is already a Comparison created with this Id");
        }
    }
}
