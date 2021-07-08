using System;
using DiffProject.Application.Enums;

namespace DiffProject.Application.Commands
{
    ///<summary>
    ///The 'Set Data' Command to be handled by the application.
    ///</summary>
    public class SetDataCommand
    {
        ///<summary>
        ///Id of the Comparison: shlould be the same for the left and right data.
        ///</summary>
        public Guid ComparisonID {get;set;}

        ///<summary>
        ///The Base64 encoded binary data.
        ///</summary>
        public String Base64BinaryData{get;set;}

        ///<summary>
        ///The Base64 encoded binary data.
        ///</summary>
        public ComparisonSideEnum SideOfComparison{get;set;}
    }
}
