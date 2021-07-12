using System;
using FluentValidation;
using FluentValidation.Results;

namespace DiffProject.Domain.AggregateModels.SeedWork
{
    /// <summary>
    /// The abstract class to be used in every domains entity. It wil handle the Id and the Validations.
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        public Entity()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Gets the Entity Id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the Fluent Validation Result.
        /// </summary>
        public ValidationResult ValidationResult { get; private set; }

        /// <summary>
        /// Executes the entity validation.
        /// </summary>
        /// <typeparam name="TModel">Type of the model to be validated.</typeparam>
        /// <param name="model">The model object to be validated.</param>
        /// <param name="validator">The validaor class containing the validation rules.</param>
        /// <returns>Returns if the validation result.</returns>
        protected bool Validate<TModel>(TModel model, AbstractValidator<TModel> validator)
        {
            ValidationResult = validator.Validate(model);
            return ValidationResult.IsValid;
        }

    }
}
