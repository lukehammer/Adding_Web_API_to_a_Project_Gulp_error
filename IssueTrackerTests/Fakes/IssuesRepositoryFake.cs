using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssueTrackerShared.Data;
using IssueTrackerShared.Models;

namespace IssueTrackerTests.Fakes
{
    public class IssuesRepositoryFake : IssuesRepository
    {
        public IssuesRepositoryFake()
            : base(null)
        {
        }

        public override void Add(Issue entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override Issue Get(int id, bool includeRelatedEntities = true)
        {
            throw new NotImplementedException();
        }

        public override IList<Issue> GetList()
        {
            throw new NotImplementedException();
        }

        public override void Update(Issue entity)
        {
            throw new NotImplementedException();
        }
    }
}
