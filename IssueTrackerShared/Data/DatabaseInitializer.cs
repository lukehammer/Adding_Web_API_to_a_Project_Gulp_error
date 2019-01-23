using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssueTrackerShared.Models;

namespace IssueTrackerShared.Data
{
    /// <summary>
    /// Custom database initializer class used to populate
    /// the database with seed data.
    /// </summary>
    internal class DatabaseInitializer : DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context context)
        {
            var departmentDesign = new Department() { Name = "Design" };
            var departmentEngineering = new Department() { Name = "Engineering" };
            var departmentMarketing = new Department() { Name = "Marketing" };
            var departmentOperations = new Department() { Name = "Operations" };
            var departmentRevenue = new Department() { Name = "Revenue" };

            var departments = new List<Department>()
            {
                departmentDesign,
                departmentEngineering,
                departmentMarketing,
                departmentOperations,
                departmentRevenue
            };

            context.Departments.AddRange(departments);

            var issues = new List<Issue>()
            {
                new Issue(2017, 8, 7, 
                    "John Smith", "john.smith@company.com", departmentOperations,
                    "My company portal login expires after only 5 minutes."),
                new Issue(2017, 8, 9,
                    "John Smith", "john.smith@company.com", departmentOperations,
                    "The public website 'About Us' page has the incorrect physical address."),
                new Issue(2017, 8, 10,
                    "Sally Jones", "sally.smith@company.com", departmentEngineering,
                    "The public website main navigation doesn't render in mobile browsers."),
                new Issue(2017, 8, 12,
                    "John Smith", "john.smith@company.com", departmentOperations,
                    "The public website renders incorrectly in IE8."),
                new Issue(2017, 8, 13,
                    "Sally Jones", "sally.smith@company.com", departmentEngineering,
                    "The public website 'Job Openings' page displays an error."),
                new Issue(2017, 8, 13,
                    "Sally Jones", "sally.smith@company.com", departmentEngineering,
                    "The public website's footer contains a broken link to the privacy policy."),
                new Issue(2017, 8, 15,
                    "John Smith", "john.smith@company.com", departmentOperations,
                    "The image carousel on the public website's home page only contains a single image."),
                new Issue(2017, 8, 16,
                    "John Smith", "john.smith@company.com", departmentOperations,
                    "The 'Company History' page on the public website contains temporary content."),
                new Issue(2017, 8, 17,
                    "John Smith", "john.smith@company.com", departmentOperations,
                    "The product search feature on the public website sometimes takes over a minute to return results."),
                new Issue(2017, 8, 19,
                    "Sally Jones", "sally.smith@company.com", departmentEngineering,
                    "The shopping cart doesn't allow you to remove items.")
            };

            context.Issues.AddRange(issues);

            context.SaveChanges();
        }
    }
}
