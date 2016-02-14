(function(){
  'use strict';

  module.exports = GroupFinderRoutes;

  GroupFinderRoutes.$inject = ['$stateProvider', '$urlRouterProvider', 'SERIES' ];

  function GroupFinderRoutes($stateProvider, $urlRouterProvider, SERIES) {

    $stateProvider
      .state(SERIES.permalink + '.host', {
        controller: 'HostCtrl as summary',
        url: '/host',
        templateUrl: 'host/host.html',
        resolve: {
          GroupQuestionService: 'GroupQuestionService',
          QuestionDefinitions: function(GroupQuestionService) {
            return GroupQuestionService.get().$promise;
          }
        },
        data: {meta: {title: SERIES.title,description: ''}}
      })
      .state(SERIES.permalink + '.host.questions', {
        controller: 'HostQuestionsCtrl as host',
        url: '/questions',
        templateUrl: 'host/host_questions.html',
        data: {meta: {title: SERIES.title,description: ''}}
      })
      .state(SERIES.permalink + '.host.review', {
        controller: 'HostReviewCtrl as host',
        url: '/review',
        templateUrl: 'host/host_review.html',
        data: {meta: {title: SERIES.title,description: ''}}
      })
      ;

  }

})();
