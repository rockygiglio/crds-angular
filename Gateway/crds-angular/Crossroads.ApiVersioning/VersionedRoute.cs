#define USE_API_VERSIONING

// To include Api Versioning,     #define USE_API_VERSIONING
// ...to leave it out,            #undef  USE_API_VERSIONING

using System;
using System.Collections.Generic;
using System.Web.Http.Routing;

namespace Crossroads.ApiVersioning
{
#if USE_API_VERSIONING
    public class VersionedRoute : RouteFactoryAttribute
    {
        public SemanticVersion MinimumVersion { get; }
        public SemanticVersion MaximumVersion { get; }
        public bool Deprecated { get; }
        public bool Removed { get; }
        public const string ApiVersionParameter = "apiVersion";
        public const string VersionedRouteConstraint = "allowedVersions";

        public VersionedRoute(string template, string minimumVersion, string maximumVersion = null, bool deprecated = false, bool removed = false)
            : base($"v{{{ApiVersionParameter}}}/{template}")
        {
            MinimumVersion = new SemanticVersion(minimumVersion);
            MaximumVersion = maximumVersion == null
                ? null
                : new SemanticVersion(maximumVersion);
            Deprecated = deprecated;
            Removed = removed;
        }

        public override IDictionary<string, object> Constraints
        {
            get
            {
                var constraints = new HttpRouteValueDictionary
                {
                    {VersionedRouteConstraint, new VersionConstraint(MinimumVersion, MaximumVersion, Deprecated, Removed)}
                };
                return constraints;
            }
        }
    }
#else
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class VersionedRoute : Attribute
    {
        public const string ApiVersionParameter = "apiVersion";

        public VersionedRoute(string template, string minimumVersion, string maximumVersion = null, bool deprecated = false, bool removed = false)
        {
        }
    }
#endif
}
