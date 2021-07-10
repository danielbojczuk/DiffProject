using System.Threading.Tasks;

namespace DiffProject.Application.CommandHandlers
{
    ///<summary>
    ///Handles the Command Set Data to perform an inclusion of a binary data to compare.
    ///</summary>
    public abstract class AbstractCommandHandler<T, K>
    {

        public abstract Task<T> ExecuteAsync(K command);

    }
}
