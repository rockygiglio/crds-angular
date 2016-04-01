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
          SkillsService: 'SkillsService',
          loggedin: crds_utilities.checkLoggedin,
          $q: '$q',
          GoVolunteerService: 'GoVolunteerService',
          Person: Person,
          PrepWork: PrepWork,
          Spouse: GetSpouse,
          Organization: Organization,
          Equipment: Equipment,
          CmsInfo: CmsInfo,
          Locations: Locations,
          ProjectTypes: ProjectTypes,
          Skills: Skills
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
          SkillsService: 'SkillsService',
          CmsInfo: CmsInfo,
          Meta: Meta,
          Organization: Organization,
          Locations: Locations,
          ProjectTypes: ProjectTypes,
          Skills: Skills,
          Equipment: Equipment,
          PrepWork: PrepWork
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
    var link = buildLink(city, organization, $state, $stateParams);
    var deferred = $q.defer();
    var page = Page.get({ url: link });
    page.$promise.then(function(data) {
      GoVolunteerService.cmsInfo = data;
      deferred.resolve();
    }, function() {

      deferred.reject();
    });

    return deferred.promise;
  }

  function Equipment(GoVolunteerService, GoVolunteerDataService, $stateParams, $q) {
    var deferred = $q.defer();
    if ($stateParams.page === 'equipment' && _.isEmpty(GoVolunteerService.availableEquipment)) {
      GoVolunteerDataService.Equipment.query(function(d) {
        GoVolunteerService.availableEquipment = d;
        deferred.resolve();
      },

      function(err) {

        console.error(err);
        deferred.reject();
      });
    } else {
      deferred.resolve();
    }

    return deferred.promise;
  }

  function GetSpouse(Profile, $cookies, $q, GoVolunteerService, $stateParams) {
    var deferred = $q.defer();

    if ($stateParams.page === 'spouse') {
      var cid = $cookies.get('userId');
      if (!cid) {
        deferred.reject();
      } else if (GoVolunteerService.spouse.preferredName === undefined) {
        Profile.Spouse.get({contactId: cid}, function(data) {
          GoVolunteerService.spouse = data;
          if (data.preferredName !== undefined) {
            GoVolunteerService.spouse.fromDb = true;
          }

          deferred.resolve();
        }, function(err) {

          console.log(err);
          deferred.reject();
        });
      } else {
        deferred.resolve();
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
      } else if (GoVolunteerService.person.nickName === '') {
        Profile.Person.get({contactId: cid}, function(data) {
          GoVolunteerService.person = data;
          deferred.resolve();
        }, function(err) {

          console.log(err);
          deferred.reject();
        });
      } else {
        deferred.resolve();
      }
    } else {
      deferred.resolve();
    }

    return deferred.promise;
  }

  function PrepWork(GoVolunteerService, GoVolunteerDataService, $stateParams, $q) {
    var deferred = $q.defer();
    if ($stateParams.page === 'available-prep' && _.isEmpty(GoVolunteerService.prepWork)) {
      GoVolunteerDataService.PrepWork.query(function(data) {
        GoVolunteerService.prepWork = data;
        deferred.resolve();
      },

      function(err) {
        deferred.reject();
      });
    } else {
      deferred.resolve();
    }

    return deferred.promise;
  }

  function Skills(GoVolunteerService, SkillsService, $stateParams, $q) {
    var deferred = $q.defer();
    if ($stateParams.page === 'unique-skills' && _.isEmpty(GoVolunteerService.skills)) {
      SkillsService.query(function(d) {
        GoVolunteerService.skills = d;
        deferred.resolve();
      },

      function(err) {

        console.err(err);
        deferred.reject();
      });

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
      Organizations.ByName.get({name: param}, function(data) {
        GoVolunteerService.organization = data;
        deferred.resolve();
      },

      function(err) {
        console.log('Error while trying to get organization ' + param);
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
      },

        function(err) {
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
      if (_.startsWith(cachedOrg.name.toLowerCase(), org.toLowerCase())) {
        return true;
      }
    }

    return false;
  }

  function buildLink(city, org, state, stateParams) {
    var base = '/go-volunteer/' + addTrailingSlashIfNecessary(city);
    if (state.next.name === 'go-volunteer.city.organizations') {
      return base + 'organizations/';
    }

    if (org) {
      return base + addTrailingSlashIfNecessary(org);
    }

    if (state.next.name === 'go-volunteer.page' || state.next.name === 'go-volunteer.crossroadspage') {
      var organization = stateParams.organization || 'crossroads';
      organization = (organization === 'other') ? 'crossroads' : organization;
      base = base + 'organizations/' + organization + '/' +  addTrailingSlashIfNecessary(stateParams.page);
    }

    return base;
  }

})();
