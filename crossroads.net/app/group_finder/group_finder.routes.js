(function(){
  'use strict';

  module.exports = GroupFinderRoutes;

  GroupFinderRoutes.$inject = ['$stateProvider', '$urlRouterProvider', 'SERIES' ];

  function GroupFinderRoutes($stateProvider, $urlRouterProvider, SERIES) {

    $stateProvider
      .state(SERIES.permalink, {
        url: '/' + SERIES.permalink,
        abstract: true,
        parent: 'noHeaderOrFooter',
        controller: 'GroupFinderCtrl as base',
        templateUrl: 'common/layout.html',
        resolve: {
          Profile: 'Profile',
          Person: 'Person',
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
        url: '/dashboard',
        templateUrl: 'dashboard/dashboard.html',
        controller: 'DashboardCtrl as dashboard',
        resolve: {
          GroupInfo: 'GroupInfo'
        },
        data: {
          meta: {
            title: SERIES.title,
            description: ''
          }
        }
      })

      .state(SERIES.permalink + '.dashboard.group', {
        url: '/groups/:groupId',
        controller: 'GroupDetailCtrl as detail',
        templateUrl: 'dashboard/group_detail.html',
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
          GroupQuestionService: require('./services/group_questions.service'),
          questions: function(GroupQuestionService) {
            return GroupQuestionService.get().$promise;
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
        url: '/host/{step:(?:[0-9]+)}',
        templateUrl: 'host/host.html',
        resolve: {
          GroupQuestionService: 'GroupQuestionService',
          QuestionDefinitions: function(GroupQuestionService) {
            return GroupQuestionService.get().$promise;
          }
        },
        data: {
          meta: {
            title: SERIES.title,
            description: ''
          }
        }
      })

      .state(SERIES.permalink + '.join_review', {
        controller: 'JoinReviewCtrl as join',
        url: '/join/results',
        templateUrl: 'join/review.html',
        resolve: {
          ParticipantQuestionService: require('./services/participant_questions.service'),
          questions: function(ParticipantQuestionService) {
            return ParticipantQuestionService.get().$promise;
          }
        },
        data: {
          meta: {
            title: SERIES.title,
            description: ''
          }
        }
      })

      .state(SERIES.permalink + '.join', {
        controller: 'JoinCtrl as join',
        url: '/join/{step:(?:[0-9]+)}',
        templateUrl: 'join/join.html',
        resolve: {
          ParticipantQuestionService: 'ParticipantQuestionService',
          QuestionDefinitions: function(ParticipantQuestionService) {
            return ParticipantQuestionService.get().$promise;
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
