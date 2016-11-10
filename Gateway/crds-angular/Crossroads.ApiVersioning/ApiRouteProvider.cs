using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using log4net;
using System.Reflection;

namespace Crossroads.ApiVersioning
{
    public class ApiRouteProvider : DefaultDirectRouteProvider
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string ApiRoutePrefix = "api";

        public IReadOnlyList<RouteEntry> DirectRoutes { get; private set; }

        public override IReadOnlyList<RouteEntry> GetDirectRoutes(HttpControllerDescriptor controllerDescriptor, IReadOnlyList<HttpActionDescriptor> actionDescriptors,
            IInlineConstraintResolver constraintResolver)
        {
            DirectRoutes = base.GetDirectRoutes(controllerDescriptor, actionDescriptors, constraintResolver);
            DetermineRouteIntegrity(controllerDescriptor);
            return DirectRoutes;
        }

        public void DetermineRouteIntegrity(HttpControllerDescriptor controllerDescriptor)
        {
            VersionSpace.Init();
            foreach(RouteEntry routeEntry in DirectRoutes)
            {
                string routePath = routeEntry.Route.RouteTemplate;
                System.Web.Http.Controllers.HttpActionDescriptor[] actions = (System.Web.Http.Controllers.HttpActionDescriptor[])routeEntry.Route.DataTokens["actions"];
                string action = actions[0].SupportedHttpMethods[0].Method;
                string route = "(" + action + ") " + routeBasename(routePath);
                VersionConstraint constraint = null;
                if (routeEntry.Route.Constraints.ContainsKey("allowedVersions"))
                    constraint = routeEntry.Route.Constraints["allowedVersions"] as VersionConstraint;
                VersionSpace.Add(route, constraint);
            }
            int routeCount = VersionSpace.Count();
            JArray problems = VersionSpace.Problems();
            if (problems.Count > 0)
                _logger.Warn("\n----------------------- Controller: " + controllerDescriptor.ControllerName + "  (" + routeCount + " routes)\nAPI Versioning Problems: \n" + problems);
        }

        private const string _versionedRoutePattern = @"^api/v\{apiVersion\}/(.*)$";
        private readonly Regex _versionedRouteRegex = new Regex(_versionedRoutePattern);
        private const string _unversionedRoutePattern = @"^api/(.*)$";
        private readonly Regex _unversionedRouteRegex = new Regex(_unversionedRoutePattern);

        private string routeBasename(string routePath)
        {
            var match = _versionedRouteRegex.Match(routePath);
            if (!match.Success)
            {
                match = _unversionedRouteRegex.Match(routePath);
                if (!match.Success)
                    return "";
            }
            return match.Groups[1].Captures[0].Value;
        }

        protected override string GetRoutePrefix(HttpControllerDescriptor controllerDescriptor)
        {
            var existingPrefix = base.GetRoutePrefix(controllerDescriptor);
            return existingPrefix == null ? ApiRoutePrefix : $"{ApiRoutePrefix}/{existingPrefix}";
        }
    }
}