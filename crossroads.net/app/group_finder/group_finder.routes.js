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
          Person: 'Person',
          User: 'User',
          GroupInfo: 'GroupInfo'
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
        url: '/dashboard',
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
