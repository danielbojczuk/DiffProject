using System;
using FluentValidation;
using FluentValidation.Results;
namespace DiffProject.Domain.AggregateModels.SeedWork
{
    ///<summary>
    ///The abstract class to be used in every domains entity. It wil handle the Id and the validations.
    ///</summary>
    public abstract class Entity
    {
        public Guid Id {get; private set;}

        public Entity() 
        {
            Id = new Guid();
        }

        public bool Validate<TModel>(TModel model, AbstractValidator<TModel> validator)
        {
            ValidationResult validation = validator.Validate(model);
            return validation.IsValid;
        }
    }
}
