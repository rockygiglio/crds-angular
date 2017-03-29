import constants from '../constants';
import { CmsInfo, Meta, GetProject, GetProfile, GetCities, GetOrganizations } from './goVolunteer.resolves';



export default function GoVolunteerRoutes($stateProvider, $urlMatcherFactoryProvider) {
  crds_utilities.preventRouteTypeUrlEncoding($urlMatcherFactoryProvider, 'goVolunteerRouteType', /\/go-volunteer\/.*$/);

  $urlMatcherFactoryProvider.caseInsensitive(true);

  // Start to reorganize routes
  $stateProvider
    .state('go-local', {
      parent: 'goCincinnati',
      abstract: true
    })
    .state('go-local.anywhereleader', {
      parent: 'goCincinnati',
      url: '/go-local/dashboard/:projectId',
      template: '<anywhere-leader></anywhere-leader>',
      data: {
        meta: {
          title: 'GO Local',
          description: ''
        },
        isProtected: true
      },
      resolve: {
        $state: '$state',
        $q: '$q',
        loggedin: crds_utilities.optimisticallyCheckLoggedin,
        GoVolunteerDataService: 'GoVolunteerDataService',
        GoVolunteerService: 'GoVolunteerService',
        GetProject
      }
    })
    .state('go-local.organizations', {
      parent: 'goCincinnati',
      url: '/go-local/:initiativeId',
      template: '<go-volunteer-organizations></go-volunteer-organizations>',
      data: {
        meta: {
          title: 'GO Local'
        }
      },
      resolve: {
        // TODO: resolve intiative to verify it is currently active
        $state: '$state',
        $q: '$q',
        GoVolunteerDataService: 'GoVolunteerDataService',
        GoVolunteerService: 'GoVolunteerService',
        GetCities,
        GetOrganizations
      }
    })
    .state('go-local.cincinnatipage', {
      parent: 'goCincinnati',
      url: '/go-local/:initiativeId/crossroads/cincinnati/:page',
      template: '<go-volunteer-page></go-volunteer-page>',
      params: {
        page: 'org-profile'
      },
      data: {
        meta: {
          title: 'GO Local',
          description: ''
        },
        isProtected: true
      },
      resolve: {
        Meta,
        Profile: 'Profile',
        Organizations: 'Organizations',
        $cookies: '$cookies',
        Session: 'Session',
        $stateParams: '$stateParams',
        SkillsService: 'SkillsService',
        loggedin: crds_utilities.optimisticallyCheckLoggedin,
        $q: '$q',
        GoVolunteerService: 'GoVolunteerService',
        Person,
        GroupFindConnectors,
        PrepWork,
        Spouse: GetSpouse,
        Organization,
        Equipment,
        ChildrenOptions,
        CmsInfo,
        Locations,
        ProjectTypes,
        Skills
      }
    })
    .state('go-local.page', {
      parent: 'goCincinnati',
      url: '/go-local/:initiativeId/:organization/cincinnati/:page',
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
        GroupFindConnectors: GroupFindConnectors,
        Organization: Organization,
        Locations: Locations,
        ProjectTypes: ProjectTypes,
        Skills: Skills,
        ChildrenOptions: ChildrenOptions,
        Equipment: Equipment,
        PrepWork: PrepWork
      }
    })
    .state('go-local.anywherepage', {
      parent: 'goCincinnati',
      url: '/go-local/:initiativeId/crossroads/:city/:projectId',
      template: '<go-volunteer-page></go-volunteer-page>',
      params: {
        page: 'anywhere-profile'
      },
      data: {
        meta: {
          title: 'GO Local',
          description: ''
        },
        isProtected: true
      },
      resolve: {
        $state: '$state',
        $q: '$q',
        loggedin: crds_utilities.optimisticallyCheckLoggedin,
        GoVolunteerDataService: 'GoVolunteerDataService',
        GoVolunteerService: 'GoVolunteerService',
        Profile: 'Profile',
        GetProject,
        GetProfile
      }
    })
    .state('go-local.anywhereconfirm', {
      parent: 'goCincinnati',
      url: '/go-local/:initiativeId/crossroads/:city/:projectId/confirm',
      template: '<go-volunteer-anywhere-profile-confirm></go-volunteer-anywhere-profile-confirm>',
      params: {
        page: 'anywhere-profile-confirm'
      },
      data: {
        meta: {
          title: 'GO Local',
          description: ''
        },
        isProtected: true
      },
      resolve: {
        $state: '$state',
        $q: '$q',
        loggedin: crds_utilities.optimisticallyCheckLoggedin,
        GoVolunteerDataService: 'GoVolunteerDataService',
        GoVolunteerService: 'GoVolunteerService',
        GetProject
      }
    })
    .state('go-local.signinpage', {
      parent: 'goCincinnati',
      url: '/go-local/:initiative/crossroads/signin',
      template: '<go-volunteer-signin> </go-volunteer-signin>',
      data: {
        meta: {
          title: 'Some Title',
          description: ''
        }
      },
      resolve: {
        $state: '$state'
      }
    });
}

function ChildrenOptions(GoVolunteerService, GoVolunteerDataService, $stateParams, $q) {
  var deferred = $q.defer();
  if ($stateParams.page === 'children-count' && _.isEmpty(GoVolunteerService.childrenOptions)) {
    GoVolunteerDataService.Children.query(function(d) {
      GoVolunteerService.childrenOptions = d;
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

  if ($stateParams.page === 'launch-site' &&
      _.isEmpty(GoVolunteerService.launchSites) &&
      GoVolunteerService.organization.organizationId) {
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



function Person(Profile, $cookies, $q, GoVolunteerService, $stateParams, Session) {
  var deferred = $q.defer();

  if ($stateParams.page === 'profile' && _.isEmpty(GoVolunteerService.person.emailAddress)) {
    var cid = $cookies.get('userId');
    if (GoVolunteerService.person.nickName === '') {
      Profile.Person.get({contactId: cid}, function(data) {
        Session.beOptimistic = true;
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
      GoVolunteerService.launchSites = {};
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

function GroupFindConnectors(GoVolunteerService, $state, $stateParams, $q, GroupConnectors) {
  var deferred = $q.defer();

  if ($stateParams.page === 'group-find-connector')  {
    if (GoVolunteerService.organization.openSignup) {
      GroupConnectors.OpenOrgs.query({initiativeId: 1}, function(data) {
        GoVolunteerService.groupConnectors = data;
        deferred.resolve();
      },

        function(err) {
          console.log(err);
          deferred.reject();
        }
      );
    } else {
      GroupConnectors.ByOrgId.query(
        {orgId: GoVolunteerService.organization.organizationId, initiativeId: 1}, function(data) {
        GoVolunteerService.groupConnectors = data;
        deferred.resolve();
      },

        function(err) {
          console.log(err);
          deferred.reject();
        }
      );
    }
  } else {
    deferred.resolve();
  }

  return deferred.promise;
}

function ProjectTypes(GoVolunteerService, $state, $stateParams, $q, GoVolunteerDataService) {
  var deferred = $q.defer();

  if ($stateParams.page === 'project-preference-one' && _.isEmpty(GoVolunteerService.projectTypes)) {
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
