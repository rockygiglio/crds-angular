

export default function WaiversRoutes($stateProvider) { /* , $urlMatcherFactoryProvider */
  // $urlMatcherFactoryProvider.caseInsensitive(true);

  $stateProvider
    .state('sign-waiver', {
      parent: 'centeredContentPage',
      url: '/waivers/send/:waiverId/:eventParticipantId',
      template: '<sign-waiver></sign-waiver>',
      data: {
        isProtected: true,
        meta: {
          title: 'Sign Waiver',
          description: ''
        }
      },
      resolve: {
        $state: '$state',
        loggedin: crds_utilies.checkLoggedin,
        waiversService: 'WaiversService',
        Waiver($state, waiversService) {
          return waiversService.getWaiver($state.toParams.waiverId);
        }
      }
    })
    // .state('accept-waiver', {
    //
    // })
    ;
}
