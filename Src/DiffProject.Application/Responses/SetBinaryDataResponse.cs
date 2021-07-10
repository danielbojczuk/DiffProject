using DiffProject.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffProject.Application.Responses
{
    public class SetBinaryDataResponse
    {
        /// <summary>
        /// Side de data should be on comparison
        /// </summary>
        public SideEnum ComparisonSide { get; set; }

        /// <summary>
        /// Base64 encoded binary Data
        /// </summary>
        public string Base64BinaryData { get; set; }

        /// <summary>
        /// ComparisonId used in both sides and result.
        /// </summary>
        public Guid ComparisonId { get; set; }

        /// <summary>
        /// Binary Data Created ID
        /// </summary>
        public Guid Id { get; set; }
    }
}
