(function() {
  'use strict';

  module.exports = JoinCompleteCtrl;

  JoinCompleteCtrl.$inject = ['$log', '$stateParams', 'Responses'];

  function JoinCompleteCtrl($log, $stateParams, Responses) {
    var vm = this;

    vm.responses = Responses;
    vm.groupId = $stateParams.groupId;

  }
})();
