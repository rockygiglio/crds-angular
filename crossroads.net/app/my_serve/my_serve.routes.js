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
        /*@ngInject*/
        leader: function (ServeTeamService) {
          return ServeTeamService.getIsLeader();
        },
        $cookies: '$cookies',
        Groups: function (ServeOpportunities, $cookies) {     
          return ServeOpportunities.ServeDays.query({
            id:   $cookies.get('userId')
          }).$promise;
        }
      }
    })
    .state('serve-signup.message', {
      parent: 'noSideBar',
      url: '/serve-signup/message',
      template: '<serve-team-message></serve-team-message>',
      resolve: {
        /*@ngInject*/
        leader: function (ServeTeamService) {
          return ServeTeamService.getIsLeader().then((data) => { ServeTeamService.isLeader = data.isLeader; });
        },
      },
      data: {
        isProtected: true,
        meta: {
          title: 'Send Message',
          description: ''
        }
      }
    });
}
