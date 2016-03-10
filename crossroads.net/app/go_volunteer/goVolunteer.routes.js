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
        parent: 'noHeaderOrFooter',
        templateUrl: 'go_volunteer/goVolunteer.template.html',
        abstract: true
      })
      .state('go-volunteer.city', {
        parent: 'go-volunteer',
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
        },
      })
      .state('go-volunteer.city.organizations', {
        parent: 'go-volunteer',
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
        parent: 'go-volunteer',
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
        parent: 'go-volunteer',
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
          $cookies: '$cookies',
          $stateParams: '$stateParams',
          loggedin: crds_utilities.checkLoggedin,
          $q: '$q',
          GoVolunteerService: 'GoVolunteerService',
          Person: Person  
        }
      })
      .state('go-volunteer.page', {
        parent: 'go-volunteer',
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
          Meta: Meta
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

  function Organization(GoVolunteerService, $stateParams, $q, crossroads) {
    var deferred = $q.defer();
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
