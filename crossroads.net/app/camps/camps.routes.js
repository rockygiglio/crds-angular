function getCampInfo(CampsService, $stateParams) {
  const id = $stateParams.campId;
  return CampsService.getCampInfo(id);
}

function getCamperInfo(CampsService, $stateParams) {
  let camperId = $stateParams.camperId;
  let campId = $stateParams.campId;
  return CampsService.getCamperInfo(campId, camperId);
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
    .state('campsignup', {
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
        $stateParams: '$stateParams',
        family: (campsService, $stateParams) => {
          const id = $stateParams.campId;
          return campsService.getCampFamily(id);
        }
      }
    })
    .state('campsignup.camper', {
      url: '/:camperId',
      template: '<camper-info></camper-info>',
      resolve: {
        campsService: 'CampsService',
        getCamperInfo: getCamperInfo,
        $stateParams: '$stateParams'
      }
    })
    .state('crossroads-camp.waivers', {
      parent: 'noSideBar',
      url: '/camps/:campId/waivers',
      template: '<camp-waivers></camp-waivers>',
      data: {
        isProtected: true,
        meta: {
          title: 'Camp Waivers',
          description: 'Join us for camp!'
        }
      }
    })
    .state('campsignup.family', {
      url: '/family',
      template: '<camps-family></camps-family>'
    });
}

