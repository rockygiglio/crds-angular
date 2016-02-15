(function(){
  'use strict';

  module.exports = JoinReviewCtrl;

  JoinReviewCtrl.$inject = ['$scope', '$state', 'Responses'];

  function JoinReviewCtrl($scope, $state, Responses) {
    var vm = this;

    vm.responses = Responses;
    vm.showUpsell = parseInt(vm.responses.data.prior_participation) > 2;
    vm.showResults = vm.showUpsell === false;

    if (vm.showResults === true) {
      $state.go('group_finder.join.results');
    }

    vm.goToHost = function() {
      $state.go('group_finder.host');
    };

    vm.goToResults = function() {
      $state.go('group_finder.join.results');
    };

    vm.contactCrds = function() {
      var zipcode = vm.responses.data.location.zip;
      // TODO utilize zipcode lookup to determine if user can be matched at all
      if (zipcode === '41075') {
        vm.showUpsell = false;
        vm.showResults = false;
        return true;
      }
    };

  }

})();
