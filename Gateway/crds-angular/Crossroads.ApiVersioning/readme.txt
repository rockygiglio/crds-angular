Crossroads Api Versioning

---------

This package provides a route versioning using semantic numbering in the form of

     Major.Minor.Revision

to identify versions.

A version number will push down to the version at or prior to an available
version. So that if version 1.3.4 is requested and versions 1.0.0, 1.2.1 and
2.0.0 of an API route are available, the version 1.2.1 will be selected.

---------

In any file using the api versioning package,

     using Crossroads.ApiVersioning;

must be included.

---------

The package has been designed to minimally impact existing code with route
attributes. An existing route attribute may look like

     [Route("api/foo/{bar}")]

that should be changed to

     [VersionedRoute(template: "foo/{bar}", mimimumVersion: "1.0.0")]

where the VersionedRoute attribute will be expanded to include the 'api' prefix,
followed by a semantic version number. If the original unversioned route should
also to be provided, it must be included

     [Route("foo/{bar}")]

and it will receive the 'api' prefix.

Optional VersionedRoute attribute parameters are

     'maximumVersion' that indicates the non-inclusive top of the version range
         that will select the route,

     'deprecated' that indicates the route is deprecated (a successful response
         will have an http status code 299 instead of 200), and

     'removed' that indicates the route has been removed (the response will have
         an http status code 410.)

When a new version of a route is created, the prior route should be given a
'maximumVersion' and the new version should use the same version as it's
'minimumVersion', like

     [ResponseType(...)]
     [VersionedRoute(template: "foo/{bar}", minimumVersion: "1.0.0",
         maximumVersion: "3.1")]
     [Route("foo/{bar}")]
     [HttpGet]
     public IHttpAction Foo(string bar)
     {
        ...
     }

     [ResponseType(...)]
     [VersionedRoute(template: "foo/{bar}", minimumVersion: "3.1")]
     [HttpGet]
     public IHttpAction FooBar(string bar)
     {
        ...
     }

Also note the 3.1 version does not include a default Route attribute, as the
defaults are only present to aid in the transition to versioning.

To keep routes looking consistent, keeping with the modern web api vernacular,
use camelCase for naming all route components, and more descriptive names
instead of less (ie. 'groupId' instead of 'id'.)

---------

Finally, in a new web app, at startup the call

     GlobalConfiguration.Configure(WebApiConfig.Register);

should be replaced with

     GlobalConfiguration.Configure(VersionConfig.Register);

to appropriately find and process the declared VersionedRoute and Route
attributes.
