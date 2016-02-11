(function() {
  'use strict';

  module.exports = GroupDetailCtrl;

  GroupDetailCtrl.$inject = ['$log', '$scope', 'GroupInfo', '$stateParams'];

  function GroupDetailCtrl($log, $scope, GroupInfo, $stateParams) {
    var vm = this;

    vm.group = GroupInfo.findHosting($stateParams.groupId);

    $log.debug(vm.group);
  }
})();
