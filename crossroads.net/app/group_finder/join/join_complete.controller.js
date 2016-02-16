(function() {
  'use strict';

  module.exports = JoinCompleteCtrl;

  JoinCompleteCtrl.$inject = ['$log', '$stateParams', 'Responses', 'Results', '$scope'];

  function JoinCompleteCtrl($log, $stateParams, Responses, Results, $scope) {
    var vm = this;

    vm.showInvite = false;
    if (Responses.data.relationship_status) {
      vm.showInvite = parseInt(Responses.data.relationship_status) === 2;
    }
    $log.debug('showInvite', vm.showInvite);
  }
})();
