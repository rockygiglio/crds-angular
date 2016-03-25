(function() {
  'use strict';

  module.exports = GoVolunteerRoutes;

  GoVolunteerRoutes.$inject = ['$stateProvider', '$urlMatcherFactoryProvider', '$locationProvider'];

  function GoVolunteerRoutes($stateProvider, $urlMatcherFactory, $locationProvider) {

    //$urlMatcherFactory.strictMode(false);

    crds_utilities.preventRouteTypeUrlEncoding($urlMatcherFactory, 'goVolunteerRouteType', /\/go-volunteer\/.*$/);

    $urlMatcherFactory.caseInsensitive(true);

    $stateProvider
      .state('go-volunteer', {
        parent: 'goCincinnati',
        abstract: true
      })
      .state('go-volunteer.city', {
        parent: 'goCincinnati',
        url: '/go-volunteer/:city',
        template: '<go-volunteer-city></go-volunteer-city>',
        data: {
          meta: {
            title: 'Some Title',
            description: ''
          }
        },
        resolve: {
          $stateParams: '$stateParams',
          Page: 'Page',
          $q: '$q',
          GoVolunteerService: 'GoVolunteerService',
          CmsInfo: CmsInfo,
          Meta: Meta
        }
      })
      .state('go-volunteer.city.organizations', {
        parent: 'goCincinnati',
        url: '/go-volunteer/:city/organizations',
        template: '<go-volunteer-organizations></go-volunteer-organizations>',
        data: {
          meta: {
            title: 'Some Title',
            description: ''
          }
        },
        resolve: {
          CmsInfo: CmsInfo,
          Meta: Meta
        }
      })
      .state('go-volunteer.signinpage', {
        parent: 'goCincinnati',
        url: '/go-volunteer/cincinnati/crossroads/signin',
        template: '<go-volunteer-signin> </go-volunteer-signin>',
        data: {
          meta: {
            title: 'Some Title',
            description: ''
          }
        },
        resolve: {
          $state: '$state',
          CmsInfo: CmsInfo,
          Meta: Meta
        }
      })
      .state('go-volunteer.crossroadspage', {
        parent: 'goCincinnati',
        url: '/go-volunteer/cincinnati/crossroads/:page',
        template: '<go-volunteer-page></go-volunteer-page>',
        data: {
          meta: {
            title: 'Some Title',
            description: ''
          },
          isProtected: true
        },
        resolve: {
          Meta: Meta,
          Profile: 'Profile',
          Organizations: 'Organizations',
          $cookies: '$cookies',
          $stateParams: '$stateParams',
          loggedin: crds_utilities.checkLoggedin,
          $q: '$q',
          GoVolunteerService: 'GoVolunteerService',
          Person: Person,
          Spouse: GetSpouse,
          Organization: Organization,
          Locations: Locations,
          ProjectTypes: ProjectTypes
        }
      })
      .state('go-volunteer.page', {
        parent: 'goCincinnati',
        url: '/go-volunteer/:city/:organization/:page',
        template: '<go-volunteer-page></go-volunteer-page>',
        data: {
          meta: {
            title: 'Some Title',
            description: ''
          }
        },
        resolve: {
          $stateParams: '$stateParams',
          $q: '$q',
          CmsInfo: CmsInfo,
          Meta: Meta,
          Organization: Organization,
          Locations: Locations,
          ProjectTypes: ProjectTypes
        }
      })
      ;
  }

  function addTrailingSlashIfNecessary(link) {
    if (_.endsWith(link, '/') === false) {
      return link + '/';
    }

    return link;
  }

  function CmsInfo(Page, $state, $stateParams, GoVolunteerService, $q) {
    var city = $stateParams.city || 'cincinnati';
    var organization = $stateParams.organizations || undefined;
    var link = buildLink(city, organization, $state);
    var deferred = $q.defer();
    var page = Page.get({ url: link });
    page.$promise.then(function(data) {
      if (data.pages.length === 0) {
        deferred.reject();
      }
      GoVolunteerService.cmsInfo = data;
      deferred.resolve();
    }, function() {
      deferred.reject();
    });

    return deferred.promise;
  }

  function GetSpouse(Profile, $cookies, $q, GoVolunteerService, $stateParams) {
    var deferred = $q.defer();

    if ($stateParams.page === 'spouse') {
      var cid = $cookies.get('userId');
      if (!cid) {
        deferred.reject();
      } else {
        Profile.Spouse.get({contactId: cid}, function(data) {
          GoVolunteerService.spouse = data;
          deferred.resolve();
        }, function(err) {

          console.log(err);
          deferred.reject();
        });
      }
    } else {
      deferred.resolve();
    }

    return deferred.promise;
  }

  function Locations($cookies, $q, GoVolunteerService, $stateParams, Organizations) {
    var deferred = $q.defer();

    if ($stateParams.page === 'launch-site') {
      Organizations.LocationsForOrg.query({orgId: GoVolunteerService.organization.organizationId}, function(data) {
        GoVolunteerService.launchSites = data;
        deferred.resolve();
      }, function(err) {
        console.log(err);
        deferred.reject();
      });
    } else {
      deferred.resolve();
    }

    return deferred.promise;
  }

  function Meta($state, $stateParams) {
    var city = $stateParams.city || 'cincinnati';
    $state.next.data.meta.title = 'GO ' + city;
  }

  function Person(Profile, $cookies, $q, GoVolunteerService, $stateParams) {
    var deferred = $q.defer();

    if ($stateParams.page === 'profile') {
      var cid = $cookies.get('userId');
      if (!cid) {
        deferred.reject();
      } else {
        Profile.Person.get({contactId: cid}, function(data) {
          GoVolunteerService.person = data;
          deferred.resolve();
        }, function(err) {

          console.log(err);
          deferred.reject();
        });
      }
    } else {
      deferred.resolve();
    }

    return deferred.promise;
  }

  function Organization(GoVolunteerService, $state, $stateParams, $q, Organizations) {
    var deferred = $q.defer();
    var param = 'crossroads'; 
    if ($state.next.name === 'go-volunteer.page') {
      param = $stateParams.organization; 
    }
    // did we already get this information?
    if (useCachedOrg(param, GoVolunteerService.organization)) {
      deferred.resolve();   
    } else {
      Organizations.ByName.get({name: param}, function(data){
        GoVolunteerService.organization = data;  
        deferred.resolve();
      }, function(err) {
        console.log('Error while trying to get organization ' + param );
        console.log(err);
        deferred.reject();
      });
    }
    return deferred.promise;
  }

  function ProjectTypes(GoVolunteerService, $state, $stateParams, $q, GoVolunteerDataService) {
    var deferred = $q.defer();
    
    if ($stateParams.page === 'project-preference-one') {
        GoVolunteerDataService.ProjectTypes.query(function(data) {
          GoVolunteerService.projectTypes = data;
          deferred.resolve();
        }, function(err) {
          console.log(err);
          deferred.reject();
        });
    } else {
      deferred.resolve();
    }

    return deferred.promise;
  }

  function useCachedOrg(org, cachedOrg) {
    if (!_.isEmpty(cachedOrg)) {
      if (_.startsWith(cachedOrg.name.toLowerCase(),org.toLowerCase())) {
        return true;
      }
    }
    return false;
  }
  
  function buildLink(city, org, state) {
    var base = '/go-volunteer/' + addTrailingSlashIfNecessary(city);
    if (state.next.name === 'go-volunteer.city.organizations') {
      return base + 'organizations/';
    }

    if (org) {
      base = base + addTrailingSlashIfNecessary(org);
    }
    return base;
  }

})();
