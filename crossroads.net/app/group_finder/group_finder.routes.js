(function(){
  'use strict';

  module.exports = GroupFinderRoutes;

  GroupFinderRoutes.$inject = ['$stateProvider', '$urlRouterProvider', 'SERIES' ];

  function GroupFinderRoutes($stateProvider, $urlRouterProvider, SERIES) {

    $stateProvider
      .state('group_finder', {
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

      .state('group_finder.welcome', {
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

      .state('group_finder.dashboard', {
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

      .state('group_finder.dashboard.group', {
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


      .state('group_finder.join_review', {
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

      .state('group_finder.join', {
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
