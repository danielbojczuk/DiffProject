using DiffProject.Application.Enums;
using DiffProject.Application.Responses;
using MediatR;
using System;

namespace DiffProject.Application.Commands
{
    ///<summary>
    ///The 'Set Data' Command to be handled by the application.
    ///</summary>
    public class SetBinaryDataCommand: IRequest<SetBinaryDataResponse>
    {
        ///<summary>
        ///Id of the Comparison: shlould be the same for the left and right data.
        ///</summary>
        public Guid ComparisonID { get; set; }

        ///<summary>
        ///The Base64 encoded binary data.
        ///</summary>
        public String Base64BinaryData { get; set; }

        ///<summary>
        ///The side to use in the comparison.
        ///</summary>
        public SideEnum ComparisonSide { get; set; }
    }
}
