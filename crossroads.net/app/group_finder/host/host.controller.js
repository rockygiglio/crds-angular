(function () {
  'use strict';

  module.exports = HostCtrl;

  HostCtrl.$inject = ['$scope'];

  function HostCtrl ($scope) {
    $scope.currentStep = 1;
  }

})();
