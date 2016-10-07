MyServeRouter.$inject = ['$httpProvider', '$stateProvider'];

export default function MyServeRouter($httpProvider, $stateProvider) {
  $stateProvider
    .state('serve-signup', {
      parent: 'noSideBar',
      url: '/serve-signup',
      controller: 'MyServeController as serve',
      templateUrl: 'my_serve/myserve.html',
      data: {
        isProtected: true,
        meta: {
          title: 'Signup to Serve',
          description: ''
        }
      },
      resolve: {
        loggedin: crds_utilities.checkLoggedin,
        ServeOpportunities: 'ServeOpportunities',
        $cookies: '$cookies',
        Groups: function(ServeOpportunities, $cookies) {
          return ServeOpportunities.ServeDays.query({
            id: $cookies.get('userId')
          }).$promise;
        }
      }
    })

  ;
}
