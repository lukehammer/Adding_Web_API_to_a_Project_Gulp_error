using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Xml.Linq;
using IssueTrackerTests.Fakes;
using Xunit;

namespace IssueTrackerTests.Helpers
{
    public abstract class BaseTests
    {
        protected const string ProjectPath = "../../../IssueTracker/";

        protected Assembly IssueTrackerAssembly { get; private set; }

        protected BaseTests(AssemblyFixture assemblyFixture)
        {
            IssueTrackerAssembly = assemblyFixture.IssueTrackerAssembly;
        }

        protected List<Package> GetPackages(string pathToProject)
        {
            var packages = XElement.Load(pathToProject + "/packages.config");
            return packages.Elements()
                .Select(p => new Package()
                {
                    Id = p.Attribute("id").Value,
                    Version = p.Attribute("version").Value,
                    TargetFramework = p.Attribute("targetFramework").Value
                })
                .ToList();
        }

        protected void FileContains(string path, string pattern, string userMessage)
        {
            Assert.True(File.Exists(path), 
                $"Does the file '{path}' exist?");

            var text = File.ReadAllText(path);

            Assert.True(Regex.IsMatch(text, pattern), userMessage);
        }
    }
}
