
using System.ComponentModel.DataAnnotations;

namespace IssueTrackerShared.Models
{
    /// <summary>
    /// Represents a department.
    /// </summary>
    public class Department
    {
        /// <summary>
        /// The ID of the department.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the department.
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}
