(function() {
  'use strict';

  module.exports = ResultsCtrl;

  ResultsCtrl.$inject = ['$log', 'Results'];

  function ResultsCtrl($log, Results) {
    var vm = this;

    vm.results = Results.getGroups();

    vm.display = function() {
      vm.results = Results.getGroups();
    };
  }
})();
