(function() {
  'use strict';

  module.exports = JoinResultsCtrl;

  JoinResultsCtrl.$inject = ['$log', 'Results'];

  function JoinResultsCtrl($log, Results) {
    var vm = this;

    vm.results = Results.getGroups();

    vm.display = function() {
      vm.results = Results.getGroups();
    };
  }
})();
