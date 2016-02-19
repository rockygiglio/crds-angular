(function () {
  'use strict';

  module.exports = JoinCtrl;

  JoinCtrl.$inject = ['$scope', 'Person'];

  function JoinCtrl ($scope, Person) {
    $scope.currentStep = 1;
    $scope.person = Person;
  }

})();
