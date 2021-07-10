using DiffProject.Application.Enums;
using System;

namespace DiffProject.Application.Commands
{
    ///<summary>
    ///The 'Set Data' Command to be handled by the application.
    ///</summary>
    public class CalculationCommand
    {
        ///<summary>
        ///The Id of the Comparison that shlould be the same for the left and right data.
        ///</summary>
        public Guid ComparisonID { get; set; }
    }
}
