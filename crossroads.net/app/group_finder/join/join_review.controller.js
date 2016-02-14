(function(){
  'use strict';

  module.exports = JoinReviewCtrl;

  JoinReviewCtrl.$inject = ['$state', 'questions', 'Responses'];

  function JoinReviewCtrl($state, questions, Responses) {
    var vm = this;

    vm.responses = Responses;
    vm.showUpsell = parseInt(vm.responses.data.member.prior_participation) > 2;
    vm.showResults = vm.showUpsell === false;

    vm.goToHost = function() {
      $state.go('group_finder.host', { step: 1 });
    };

    vm.goToResults = function() {
      vm.showUpsell = false;
      vm.showResults = true;
    };

  }

})();
