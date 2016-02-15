(function() {
  'use strict';

  module.exports = JoinModalCtrl;

  JoinModalCtrl.$inject = ['$scope', '$state', '$modalInstance', 'groupId'];

  function JoinModalCtrl($scope, $state, $modalInstance, groupId) {
    var vm = this;
    vm.join = function() {
      // TODO integrate person -> group service here
      $modalInstance.close($scope.confirmed = true);
      $state.go('^.complete', { 'groupId': $scope.groupId});
    };

    vm.cancel = function() {
      $modalInstance.close();
    };
  }
})();
