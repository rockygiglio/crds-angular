(function(){
  'use strict';

  module.exports = HostReviewCtrl;

  HostReviewCtrl.$inject = ['$state', 'questions', 'Responses'];

  function HostReviewCtrl($state, questions, Responses) {
    var vm = this;

    vm.questions = questions;
    vm.responses = Responses;

    vm.startOver = function(){
      vm.currentQuestion = 1;
      $state.go('brave.host', { step: vm.currentQuestion });
    };

  }

})();
