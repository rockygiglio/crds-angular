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
          var now = new Date();
          var weekOut = new Date();
          weekOut.setDate(now.getDate() + 7);
          var from = Math.floor(now.getTime()/1000);
          var to = Math.floor(weekOut.getTime()/1000);

          return ServeOpportunities.ServeDays.query({
            id:   $cookies.get('userId'),
            from: from ,
            to:   to 
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
