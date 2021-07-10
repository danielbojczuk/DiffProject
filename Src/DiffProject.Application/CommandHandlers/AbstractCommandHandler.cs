using System;
using System.Threading.Tasks;
using DiffProject.Application.Commands;
using DiffProject.Application.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;

namespace DiffProject.Application.CommandHandlers
{
    ///<summary>
    ///Handles the Command Set Data to perform an inclusion of a binary data to compare.
    ///</summary>
    public abstract class AbstractCommandHandler<T,K>
    {

        public abstract Task<T> ExecuteAsync(K command);
  
    }
}
