(function () {
  'use strict';

  module.exports = HostCtrl;

  HostCtrl.$inject = ['$scope', '$state', 'AuthService', 'Person'];

  function HostCtrl ($scope, $state, AuthService, Person) {
    $scope.currentStep = 1;
    $scope.person = Person;
  }

})();
