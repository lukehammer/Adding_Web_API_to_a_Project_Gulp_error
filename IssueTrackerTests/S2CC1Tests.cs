using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using IssueTrackerTests.Helpers;
using Xunit;

namespace IssueTrackerTests
{
    [Collection("Test Collection")]
    [Trait("Code Challenge", "S2CC1")]
    public class S2CC1Tests : BaseTests
    {
        public S2CC1Tests(AssemblyFixture assemblyFixture)
            : base(assemblyFixture)
        {
        }

        [Fact]
        public void ExpectedPackagesHaveBeenInstalled()
        {
            var webApiPackage = 
                GetPackages(ProjectPath)
                .Where(p => p.Id == "Microsoft.AspNet.WebApi")
                .FirstOrDefault();

            Assert.True(
                webApiPackage != null, 
                "Did you use the NuGet Package Manager to install the 'Microsoft.AspNet.WebApi' package?");

            Assert.True(
                webApiPackage.Version == "5.2.3", 
                "Did you install version 5.2.3 of the 'Microsoft.AspNet.WebApi' package?");
        }

        [Fact]
        public void WebApiConfigFileIsPresentInTheExpectedFolder()
        {
            Assert.True(
                Directory.Exists(ProjectPath + "App_Start"), 
                "Do you have an 'App_Start' folder in the root of your project?");

            Assert.True(
                File.Exists(ProjectPath + "App_Start/WebApiConfig.cs"), 
                "Did you add a 'WebApiConfig.cs' file to the 'App_Start' folder?");
        }

        [Fact]
        public void WebApiConfigClassIsDefined()
        {
            var webApiConfigClass = IssueTrackerAssembly
                .ShouldHaveType("IssueTracker.WebApiConfig")
                .ThatIsPublic()
                .AndStatic();

            var registerMethod = webApiConfigClass
                .ShouldHaveMethod(
                    "Register", 
                    new[] { new Parameter(typeof(HttpConfiguration), "config") }, 
                    validateParameterNames: true)
                .ThatIsPublic()
                .AndStatic()
                .AndReturnsVoid();
        }

        [Fact]
        public void GlobalAsaxCsFileIsPresentInTheExpectedFolder()
        {
            Assert.True(
                File.Exists(ProjectPath + "Global.asax.cs"),
                "Is the 'Global.asax.cs' file available in the root of the project?");
        }

        [Fact]
        public void ApplicationStartMethodCallsTheGlobalConfigurationConfigureMethod()
        {
            var webApiApplicationClass = IssueTrackerAssembly
                .ShouldHaveType("IssueTracker.WebApiApplication")
                .ThatIsPublic()
                .AndNonStatic()
                .AndInheritsFrom(typeof(System.Web.HttpApplication));

            var applicationStartMethod = webApiApplicationClass
                .ShouldHaveMethod("Application_Start")
                .ThatIsProtected()
                .AndNonStatic()
                .AndReturnsVoid();

            FileContains(ProjectPath + "Global.asax.cs",
                @"protected\s+void\s+Application_Start\s*\(\s*\)\s*\{\s*GlobalConfiguration\.Configure\s*\(\s*WebApiConfig\.Register\s*\)\s*;\s*\}", 
                "In the 'WebApiApplication.Application_Start' method, are you making a call to the 'GlobalConfiguration.Configure' method and passing a reference to the 'WebApiConfig.Register' method?");
        }
    }
}
