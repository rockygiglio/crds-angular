(function() {
  'use strict';

  module.exports = JoinResultsCtrl;

  JoinResultsCtrl.$inject = ['Results', '$scope', '$anchorScroll'];

  function JoinResultsCtrl(Results, $scope, $anchorScroll) {
    var vm = this;

    vm.results = Results.getResults();
    vm.currentPage = 1;
    vm.numPerPage = 6;

    $scope.$watch('result.currentPage + result.numPerPage', function() {
      var begin = ((vm.currentPage - 1) * vm.numPerPage);
      var end = begin + vm.numPerPage;
      vm.filteredResults = vm.results.slice(begin, end);
      $anchorScroll();
    });
  }
})();
