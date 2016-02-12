(function(){
  'use strict';

  module.exports = JoinReviewCtrl;

  JoinReviewCtrl.$inject = ['$state', 'questions', 'Responses', 'SERIES'];

  function JoinReviewCtrl($state, questions, Responses, SERIES) {
    var vm = this;

    vm.responses = Responses;
    vm.showUpsell = parseInt(vm.responses.data.member.prior_participation) > 2;
    vm.showResults = vm.showUpsell === false;

    vm.goToHost = function() {
      $state.go(SERIES.permalink + '.host', { step: 1 });
    };

    vm.goToResults = function() {
      vm.showUpsell = false;
      vm.showResults = true;
    };

  }

})();
