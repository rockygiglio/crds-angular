function getCampInfo(CampsService, $stateParams) {
  const id = $stateParams.campId;
  return CampsService.getCampInfo(id);
}

export default function CampRoutes($stateProvider) {
  $stateProvider
    .state('camps-dashboard', {
      parent: 'noSideBar',
      url: '/mycamps',
      template: '<camps-dashboard dashboard="$resolve.dashboard"></camps-dashboard>',
      data: {
        isProtected: true,
        meta: {
          title: 'Camps Dashboard',
          description: 'What camps are you signed up for?'
        }
      },
      resolve: {
        loggedin: crds_utilities.checkLoggedin,
        campsService: 'CampsService',
        dashboard: campsService => campsService.getCampDashboard()
      }
    })
    .state('summercamp', {
      parent: 'noSideBar',
      url: '/camps/summercamp',
      template: '<crossroads-camp is-summer-camp="true"></crossroads-camp>',
      data: {
        isProtected: true,
        meta: {
          title: 'Summer Camp',
          description: 'Signup for Summer Camp'
        }
      },
      resolve: {
        loggedin: crds_utilities.checkLoggedin
      }
    })
    .state('crossroads-camp', {
      parent: 'noSideBar',
      url: '/camps/:campId',
      template: '<crossroads-camp></crossroads-camp>',
      data: {
        isProtected: true,
        meta: {
          title: 'Camp Signup',
          description: 'Join us for camp!'
        }
      },
      resolve: {
        loggedin: crds_utilities.checkLoggedin,
        campsService: 'CampsService',
        getCampInfo,
        $stateParams: '$stateParams'
      }
    });
}
