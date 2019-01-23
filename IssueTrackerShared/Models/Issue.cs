using System;
using System.ComponentModel.DataAnnotations;

namespace IssueTrackerShared.Models
{
    /// <summary>
    /// Represents a reported issue.
    /// </summary>
    public class Issue
    {
        /// <summary>
        /// The severity level of the issue.
        /// </summary>
        public enum SeverityLevel
        {
            Minor,
            Major,
            Critical
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Issue()
        {
        }

        /// <summary>
        /// Constructor for creating issues.
        /// </summary>
        /// <param name="year">The year (1 through 9999) for the issue reported on date.</param>
        /// <param name="month">The month (1 through 12) for the issue reported on month.</param>
        /// <param name="day">The day (1 through the number of days for the month) for the issue reported on day.</param>
        /// <param name="name">The name of the person reporting the issue.</param>
        /// <param name="email">The email for the person reporting the issue.</param>
        /// <param name="department">The department of the person reporting the issue.</param>
        /// <param name="descriptionOfProblem">The description of the problem.</param>
        /// <param name="severity">The severity level for the issue.</param>
        /// <param name="reproducible">Whether or not the issue is reproducible.</param>
        public Issue(int year, int month, int day, string name, string email, 
            Department department, string descriptionOfProblem, 
            SeverityLevel severity = SeverityLevel.Minor, bool reproducible = true)
        {
            ReportedOn = new DateTime(year, month, day);
            Name = name;
            Email = email;
            Department = department;
            DescriptionOfProblem = descriptionOfProblem;
            Severity = severity;
            Reproducible = reproducible;
        }

        /// <summary>
        /// The ID of the issue.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The date/time that the issue was reported on.
        /// </summary>
        public DateTime ReportedOn { get; set; }

        /// <summary>
        /// The name of the person reporting the issue.
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// The email for the person reporting the issue.
        /// </summary>
        [Required, MaxLength(255)]
        public string Email { get; set; }

        /// <summary>
        /// The department ID for the issue. The ID value should map to an ID in the departments collection.
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// The department for the issue.
        /// </summary>
        public Department Department { get; set; }

        /// <summary>
        /// The description of the problem.
        /// </summary>
        [Required]
        public string DescriptionOfProblem { get; set; }

        /// <summary>
        /// The severity level for the issue.
        /// </summary>
        public SeverityLevel Severity { get; set; }

        /// <summary>
        /// Whether or not the issue is reproducible.
        /// </summary>
        public bool Reproducible { get; set; }

        /// <summary>
        /// The severity level of the issue as a string.
        /// </summary>
        public string SeverityName => Severity.ToString();
    }
}
