import { invokeResolve } from './application_page/resolve_registry';

import { getCampInfo, getCamperFamily, getCamperPayment } from './camps.resolves';

export default function CampRoutes($stateProvider) {
  $stateProvider
    .state('camps-dashboard', {
      parent: 'noSideBar',
      url: '/mycamps?invoiceId&paymentId',
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
        $state: '$state',
        $filter: '$filter',
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
        $stateParams: '$stateParams'
      }
    })
    .state('campsignup.family', {
      url: '/family',
      template: '<camps-family></camps-family>',
      resolve: {
        $stateParams: '$stateParams',
        campsService: 'CampsService',
        getCamperFamily
      }
    })
    .state('campsignup.confirmation', {
      url: '/confirmation/:contactId?paymentId&invoiceId',
      resolve: {
        CampsService: 'CampsService',
        $state: '$state',
        $timeout: '$timeout',
        sendConfirmation: (CampsService, $state, $timeout) => CampsService.sendConfirmation($state.toParams.invoiceId,
            $state.toParams.paymentId,
            $state.toParams.campId,
            $state.toParams.contactId)
            .then(() => {
              // When the confirmation API calls returns, forward to the thank you page
              $timeout(() => {
                $state.go('campsignup.thankyou', $state.toParams, { location: 'replace' });
              }, 0);
            })
      }
    })
    .state('campsignup.thankyou', {
      url: '/thankyou/:contactId?paymentId&invoiceId',
      template: '<camp-thank-you></camp-thank-you>',
      resolve: {
        CampsService: 'CampsService',
        $state: '$state',
        getCamperPayment,
        getCamperFamily
      }
    })
    //
    // THIS MUST BE THE LAST DEFINED STATE
    //
    .state('campsignup.application', {
      url: '/:page/:contactId',
      template: '<camps-application-page></camps-application-page>',
      params: {
        update: false,
        redirectTo: undefined
      },
      resolve: {
        $injector: '$injector',
        $state: '$state',
        $q: '$q',
        $timeout: '$timeout',
        $log: '$log',
        $stateParams: '$stateParams',
        CampsService: 'CampsService',
        register: invokeResolve
      }
    })
    ;
}
