(function(){
  'use strict';

  module.exports = GroupFinderRoutes;

  GroupFinderRoutes.$inject = ['$stateProvider', '$urlRouterProvider', 'SERIES'];

  function GroupFinderRoutes($stateProvider, $urlRouterProvider, SERIES) {

    $stateProvider
      .state(SERIES.permalink, {
        url: '/' + SERIES.permalink,
        abstract: true,
        templateUrl: 'common/layout.html',
        resolve: {
          Profile: 'Profile',
          $cookies: '$cookies',
          Person: function(Profile, $cookies) {
            var cid = $cookies.get('userId');
            if (cid) {
              return Profile.Person.get({contactId: cid}).$promise;
            }
          },
          User: function($http){
            // TODO Update to use $resource
            return $http.get('/app/group_finder/data/user.group.json')
              .then(function(res){
                return res.data;
              });
          }
        },
        data: {
          meta: {
            title: SERIES.title,
            description: ''
          }
        }
      })

      .state(SERIES.permalink + '.welcome', {
        controller: 'LoginCtrl as ctrl',
        url: '/welcome',
        templateUrl: 'login/welcome.html',
        resolve: {},
        data: {
          meta: {
            title: SERIES.title,
            description: ''
          }
        }
      })

      .state(SERIES.permalink + '.dashboard', {
        controller: 'DashboardCtrl as dashboard',
        url: '/host/dashboard',
        templateUrl: 'dashboard/dashboard.html',
        resolve: {},
        data: {
          meta: {
            title: SERIES.title,
            description: ''
          }
        }
      })

      .state(SERIES.permalink + '.summary', {
        controller: 'SummaryCtrl as summary',
        url: '/summary',
        templateUrl: 'summary/summary.html',
        resolve: {},
        data: {
          meta: {
            title: SERIES.title,
            description: ''
          }
        }

      })

      .state(SERIES.permalink + '.host_review', {
        controller: 'HostReviewCtrl as host',
        url: '/host/review',
        templateUrl: 'host/review.html',
        resolve: {
          QuestionService: require('./services/group_questions.service'),
          questions: function(QuestionService) {
            return QuestionService.get().$promise;
          }
        },
        data: {
          meta: {
            title: SERIES.title,
            description: ''
          }
        }
      })

      .state(SERIES.permalink + '.host', {
        controller: 'HostCtrl as host',
        url: '/host/{step:(?:[0-9])}',
        templateUrl: 'host/host.html',
        resolve: {
          QuestionService: 'QuestionService',
          QuestionDefinitions: function(QuestionService) {
            return QuestionService.get().$promise;
          }
        },
        data: {
          meta: {
            title: SERIES.title,
            description: ''
          }
        }
      })

    ;

    $urlRouterProvider
      .when('/' + SERIES.permalink, '/' + SERIES.permalink + '/welcome')
      .otherwise('/' + SERIES.permalink + '/welcome');

  }

})();
