export default function ChildcareRoutes($stateProvider) {
  $stateProvider.state('childcare-dashboard', {
    parent: 'noSideBar',
    url: '/childcare-dashboard',
    template: '<childcare-dashboard></childcare-dashboard>',
    data: {
      isProtected: true,
      meta: {
        title: 'Childcare Dashboard',
        description: ''
      }
    },
    resolve: {
      loggedin: crds_utilities.checkLoggedin,
      $cookies: '$cookies',
    }
  });
}
