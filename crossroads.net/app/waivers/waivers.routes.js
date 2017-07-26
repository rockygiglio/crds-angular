

export default function WaiversRoutes($stateProvider) { /* , $urlMatcherFactoryProvider */
  // $urlMatcherFactoryProvider.caseInsensitive(true);

  $stateProvider
    .state('sign-waiver', {
      parent: 'centeredContentPage',
      url: '/waivers/sign/:waiverId/:eventParticipantId',
      template: '<sign-waiver></sign-waiver>',
      data: {
        isProtected: true,
        meta: {
          title: 'Sign Waiver',
          description: ''
        }
      },
      resolve: {
        $log: '$log',
        $rootScope: '$rootScope',
        $state: '$state',
        loggedin: crds_utilities.checkLoggedin,
        waiversService: 'WaiversService'
      }
    })
    .state('accept-waiver', {
      parent: 'noHeaderOrFooter',
      url: '/waivers/accept/:waiverId/:eventParticipantId',
      template: '<accept-waiver></accept-waiver>',
      controller: ($log, $rootScope, $state, waiversService) => {
        const { waiverId, eventParticipantId } = $state.params;

        waiversService.acceptWaiver(waiverId, eventParticipantId).then(() => {
          $state.go('mytrips');
        }).catch((err) => {
          $log.error(err);
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        });
      },
      data: {
        isProtected: true,
        meta: {
          title: 'Accept Waiver',
          description: ''
        }
      },
      resolve: {
        $log: '$log',
        $rootScope: '$rootScope',
        $state: '$state',
        loggedin: crds_utilities.checkLoggedin,
        waiversService: 'WaiversService'
      }
    });
}
