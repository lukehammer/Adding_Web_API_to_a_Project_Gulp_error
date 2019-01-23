using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IssueTrackerTests.Helpers
{
    public class RouteData
    {
        public string RouteName { get; set; }
        public string Controller { get; set; }
        public int? Id { get; set; }
        public string AbsoluteUri { get; set; }
        public string RouteTemplate { get; set; }

        public string GetLocation(string routeName, object routeValues)
        {
            if (routeName != RouteName || routeValues == null)
            {
                return null;
            }

            var routeValuesType = routeValues.GetType();
            var propertyBindingFlags =
                BindingFlags.IgnoreCase |
                BindingFlags.Instance |
                BindingFlags.NonPublic |
                BindingFlags.Public;

            string controller = string.Empty;
            var controllerProperty = routeValuesType.GetProperty("controller", propertyBindingFlags);

            if (controllerProperty != null)
            {
                var controllerPropertyValue = controllerProperty.GetValue(routeValues);

                if (controllerPropertyValue.GetType() == typeof(string))
                {
                    controller = (string)controllerPropertyValue;
                }
            }

            int id = 0;
            var idProperty = routeValuesType.GetProperty("id", propertyBindingFlags);

            if (idProperty != null)
            {
                var idPropertyValue = idProperty.GetValue(routeValues);

                if (idPropertyValue.GetType() == typeof(int))
                {
                    id = (int)idPropertyValue;
                }
            }

            string location = "http://localhost/";

            var routeTemplate = RouteTemplate;

            if (!string.IsNullOrWhiteSpace(controller))
            {
                location += routeTemplate
                    .Replace("{controller}", controller.ToLower())
                    .Replace("/{id}", id > 0 ? "/" + id.ToString() : string.Empty);
            }

            return location;
        }
    }
}
