(function(){
  'use strict';

  module.exports = HostReviewCtrl;

  HostReviewCtrl.$inject = ['$scope', '$state', 'Responses', 'SERIES'];

  function HostReviewCtrl($scope, $state, Responses, SERIES) {
    var vm = this;

    vm.responses = Responses;

    vm.startOver = function() {
      $scope.$parent.currentStep = 2;
      $state.go(SERIES.permalink + '.host.questions');
    };

    vm.showDashboard = function() {
      $state.go(SERIES.permalink + '.dashboard');
    };

  }

})();
