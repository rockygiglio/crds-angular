(function(){
  'use strict';

  module.exports = JoinReviewCtrl;

  JoinReviewCtrl.$inject = ['$scope', '$state', 'Responses'];

  function JoinReviewCtrl($scope, $state, Responses) {
    var vm = this;

    vm.responses = Responses;
    vm.showUpsell = parseInt(vm.responses.data.member.prior_participation) > 2;
    vm.showResults = vm.showUpsell === false;

    vm.goToHost = function() {
      $state.go('group_finder.host');
    };

    vm.goToResults = function() {
      vm.showUpsell = false;
      vm.showResults = true;
    };

    vm.startOver = function() {
      $scope.$parent.currentStep = 2;
      $state.go('group_finder.host.questions');
    };

    vm.showDashboard = function() {
      $state.go('group_finder.dashboard');
    };
  }

})();
