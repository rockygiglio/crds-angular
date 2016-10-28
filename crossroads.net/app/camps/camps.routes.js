import campWaiversResolve from './camp_waivers/camp_waivers.resolve';

function getCampInfo(CampsService, $stateParams) {
  const id = $stateParams.campId;
  return CampsService.getCampInfo(id);
}

function getCamperInfo(CampsService, $stateParams) {
  const camperId = $stateParams.camperId;
  const campId = $stateParams.campId;
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
      },
      resolve: campWaiversResolve
    })
    .state('campsignup.family', {
      url: '/family',
      template: '<camps-family></camps-family>',
    })
    .state('campsignup.application', {
      url: '/:page/:contactId',
      template: '<camps-application-page></camps-application-page>'
    })
    .state('campsignup.camper', {
      url: '/:camperId',
      template: '<camper-info></camper-info>',
      resolve: {
        campsService: 'CampsService',
        getCamperInfo,
        $stateParams: '$stateParams'
      }
    });
}
