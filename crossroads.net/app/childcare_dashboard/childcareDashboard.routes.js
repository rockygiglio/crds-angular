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
      ChildcareDashboardService: 'ChildcareDashboardService',
      loggedin: crds_utilities.checkLoggedin,
      $cookies: '$cookies',
      $q: '$q',
      FetchChildcareDates: fetchChildcareDates
    }
  });

  function fetchChildcareDates(ChildcareDashboardService, $q) {
    var deferred = $q.defer();
    var cds = ChildcareDashboardService.fetchChildcareDates();
    cds.$promise.then((data) => {
      console.log(data);
      ChildcareDashboardService.childcareDates = data;
      deferred.resolve();
    }, (err) => {
      if (err.status === 406) {
        ChildcareDashboardService.headOfHouseholdError = true;
        deferred.resolve();
      }
      deferred.reject();
    });
    return deferred.promise;
  }

}
