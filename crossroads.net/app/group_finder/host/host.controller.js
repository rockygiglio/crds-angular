(function () {
  'use strict';

  module.exports = HostCtrl;

  HostCtrl.$inject = ['$scope', '$state', 'Person'];

  function HostCtrl ($scope, $state, Person) {
    $scope.currentStep = 1;
    $scope.person = Person;
  }

})();
