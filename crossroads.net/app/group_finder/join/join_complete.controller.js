(function() {
  'use strict';

  module.exports = JoinCompleteCtrl;

  JoinCompleteCtrl.$inject = ['$log', '$stateParams', 'Responses', 'Results', '$scope'];

  function JoinCompleteCtrl($log, $stateParams, Responses, Results, $scope) {
    var vm = this;

    vm.error = $scope.error;

  }
})();
