using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;

namespace IssueTrackerTests.Helpers
{
    public class ApiControllerMock<TControllerType, TRepositoryType> 
        : ObjectWithMockedDependency<TControllerType, TRepositoryType>
        where TControllerType : ApiController
        where TRepositoryType : class
    {
        public ApiControllerMock(RouteData locationRouteData = null)
        {
            Object.Request = new HttpRequestMessage();
            Object.Configuration = new HttpConfiguration();

            if (locationRouteData != null)
            {
                var urlMock = new Mock<UrlHelper>();
                urlMock.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>()))
                    .Returns<string, object>((routeName, routeValues) =>
                    {
                        return locationRouteData.GetLocation(routeName, routeValues);
                    });
                Object.Url = urlMock.Object;
                UrlHelperMock = urlMock;
            }
        }

        public Mock<UrlHelper> UrlHelperMock { get; private set; }
    }
}
