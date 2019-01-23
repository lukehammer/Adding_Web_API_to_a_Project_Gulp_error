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
    /// The repository for issues.
    /// </summary>
    public class IssuesRepository : BaseRepository<Issue>
    {
        public IssuesRepository(Context context)
            : base(context)
        {
        }

        /// <summary>
        /// Returns a single issue for the provided ID.
        /// </summary>
        /// <param name="id">The ID for the issue to return.</param>
        /// <param name="includeRelatedEntities">Indicates whether or not to include related entities.</param>
        /// <returns>An issue.</returns>
        public override Issue Get(int id, bool includeRelatedEntities = true)
        {
            var issues = Context.Issues.AsQueryable();

            if (includeRelatedEntities)
            {
                issues = issues
                    .Include(i => i.Department);
            }

            return issues
                .Where(e => e.Id == id)
                .SingleOrDefault();
        }

        /// <summary>
        /// Returns a collection of issues.
        /// </summary>
        /// <returns>A list of issues.</returns>
        public override IList<Issue> GetList()
        {
            return Context.Issues
                .Include(i => i.Department)
                .OrderByDescending(i => i.ReportedOn)
                .ThenByDescending(i => i.Id)
                .ToList();
        }
    }
}
