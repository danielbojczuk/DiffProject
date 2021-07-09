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
        public virtual Guid Id {get; private set;}

        public Entity() 
        {
            Id = new Guid();
        }

        public Entity(Guid id)
        {
            Id = id;
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
            ValidationResult validation = validator.Validate(model);
            return validation.IsValid;
        }
    }
}
