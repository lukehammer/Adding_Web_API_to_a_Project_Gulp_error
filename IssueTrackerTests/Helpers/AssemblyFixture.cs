using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IssueTrackerTests.Helpers
{
    public class AssemblyFixture
    {
        public Assembly IssueTrackerAssembly { get; set; }

        public AssemblyFixture()
        {
            IssueTrackerAssembly = LoadAssembly("IssueTracker");
        }

        private Assembly LoadAssembly(string name)
        {
            var assembly = Assembly.Load($"{name}, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

            //Assert.True(assembly != null,
            //    $"Does the solution contain a project named '{name}'?");

            return assembly;
        }
    }
}
