(function(){
  'use strict';

  module.exports = HostReviewCtrl;

  HostReviewCtrl.$inject = ['$state', 'questions', 'Responses', 'SERIES'];

  function HostReviewCtrl($state, questions, Responses, SERIES) {
    var vm = this;

    vm.questions = questions;
    vm.responses = Responses;

    vm.startOver = function() {
      vm.currentQuestion = 1;
      $state.go(SERIES.permalink + '.host', { step: vm.currentQuestion });
    };

    vm.showDashboard = function() {
      $state.go(SERIES.permalink + '.dashboard');
    };

  }

})();
