using DiffProject.Application.Enums;
using System;

namespace DiffProject.Application.Commands
{
    ///<summary>
    ///The 'Set Data' Command to be handled by the application.
    ///</summary>
    public class UpdateBinaryDataCommand
    {
        ///<summary>
        ///The Id of the Comparison that shlould be the same for the left and right data.
        ///</summary>
        public Guid CurrentComparisonID { get; set; }

        ///<summary>
        ///The NEW Base64 encoded binary data.
        ///</summary>
        public String NewBase64BinaryData { get; set; }

        ///<summary>
        ///The current Side of the binary data
        ///</summary>
        public SideEnum CurrentComparisonSide { get; set; }
    }
}
