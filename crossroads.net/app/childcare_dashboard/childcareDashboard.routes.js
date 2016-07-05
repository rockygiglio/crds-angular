export default function ChildcareRoutes($stateProvider) {
  $stateProvider.state('childcare-dashboard', {
    parent: 'noSideBar',
    url: '/childcare-dashboard',
    controller: 'ChildcareDashboardController as childcaredashboard',
    templateUrl: 'childcare_dashboard/childcareDashboard.html',
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
