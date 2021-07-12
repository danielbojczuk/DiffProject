using FluentValidation;
using FluentValidation.Results;
using System;
namespace DiffProject.Domain.AggregateModels.SeedWork
{
    ///<summary>
    ///The abstract class to be used in every domains entity. It wil handle the Id and the validations.
    ///</summary>
    public abstract class Entity
    {
        public Guid Id { get; private set; }

        /// <summary>
        /// Property to keep the Fluent validation status
        /// </summary>
        public ValidationResult ValidationResult { get; private set; }

        public Entity()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Execute the validation set with FluentValidation
        /// </summary>
        /// <typeparam name="TModel">Type of the model to be validated</typeparam>
        /// <param name="model">The model object to be validated</param>
        /// <param name="validator">The validaor class containing the validation rules</param>
        /// <returns></returns>
        protected bool Validate<TModel>(TModel model, AbstractValidator<TModel> validator)
        {
            ValidationResult = validator.Validate(model);
            return ValidationResult.IsValid;
        }

    }
}
