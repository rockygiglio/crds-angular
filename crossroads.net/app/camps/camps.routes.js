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
        dashboard: (campsService) => campsService.getCampDashboard()
      }
    })
    .state('crossroads-camp', {
      parent: 'noSideBar',
      url: '/camps/:campId',
      template:'<crossroads-camp></crossroads-camp>',
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
        getCampInfo: getCampInfo,
        $stateParams: '$stateParams'
      }
    });
}

function getCampInfo(CampsService, $stateParams) {
  let id = $stateParams.campId;
  return CampsService.getCampInfo(id);
}
