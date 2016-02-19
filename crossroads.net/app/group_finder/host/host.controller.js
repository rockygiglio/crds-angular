(function () {
  'use strict';

  module.exports = HostCtrl;

  HostCtrl.$inject = ['$timeout', '$scope', '$state', 'Person'];

  function HostCtrl ($timeout, $scope, $state, Person) {
    $scope.currentStep = 1;
    $scope.person = Person;
  }

})();
