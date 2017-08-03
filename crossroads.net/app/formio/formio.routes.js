

export default function FormioRoutes($stateProvider) {
  $stateProvider
    .state('formio', {
      parent: 'noSideBar',
      url: '/formio/:formPath',
      template: '<crds-formio></crds-formio>',
      // component: 'crdsFormio',
      data: {
        isProtected: true,
        meta: {
          title: 'Crds Formio',
          description: ''
        }
      },
      resolve: {
        $state: '$state'
      }
    });
}
