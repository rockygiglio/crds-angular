

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
        $state: '$state',
        loggedin: crds_utilities.checkLoggedin,
        waiversService: 'WaiversService'
      }
    })
    // .state('accept-waiver', {
    //
    // })
    ;
}
