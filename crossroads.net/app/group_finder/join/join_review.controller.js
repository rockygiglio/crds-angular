(function(){
  'use strict';

  module.exports = JoinReviewCtrl;

  JoinReviewCtrl.$inject = ['$scope', '$state', 'Responses', '$log'];

  function JoinReviewCtrl($scope, $state, Responses, $log) {
    var vm = this;

    vm.responses = Responses;
    vm.showUpsell = parseInt(vm.responses.data.prior_participation) > 2;
    vm.showResults = vm.showUpsell === false;
    vm.contactCrds = false;

    if (vm.responses.data.location && vm.responses.data.location.zip === '41075') {
      vm.showUpsell = false;
      vm.showResults = false;
      vm.contactCrds = true;
    }

    if (vm.showResults === true && vm.contactCrds === false) {
      $state.go('group_finder.join.results');
    }

    if (parseInt(vm.responses.data.relationship_status) === 2) {
      $log.debug('married');
      $scope.showInvite = true;
    }

    vm.goToHost = function() {
      $state.go('group_finder.host');
    };

    vm.goToResults = function() {
      $state.go('group_finder.join.results');
    };

  }

})();
