using System;
using System.Threading.Tasks;
using DiffProject.Application.Commands;
namespace DiffProject.Application.CommandHandlers
{
    ///<summary>
    ///Handles the Command Set Data to perform an inclusion of a binary data to compare.
    ///</summary>
    public class SetDataCommandHandler
    {
        ///<summary>
        ///Execute Async the 'Set Data' Command
        ///</summary>
        ///<param name="command">Command to be handled with the Comparison Id and the Bas64 Binary Data</param>
        public async Task<Guid> ExecuteAsync(SetDataCommand command)
        {

            return Guid.NewGuid();
            
        }
    }
}
