using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssueTrackerShared.Models;

namespace IssueTrackerShared.Data
{
    /// <summary>
    /// Repository for departments.
    /// </summary>
    public class DepartmentsRepository : BaseRepository<Department>
    {
        public DepartmentsRepository(Context context)
            : base(context)
        {
        }

        public override Department Get(int id, bool includeRelatedEntities = true)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a collection of departments.
        /// </summary>
        /// <returns>A list of departments.</returns>
        public override IList<Department> GetList()
        {
            return Context.Departments
                .OrderBy(a => a.Name)
                .ToList();
        }
    }
}
