(function() {
  'use strict';

  module.exports = JoinResultsCtrl;

  JoinResultsCtrl.$inject = [
    'Results',
    '$scope',
    '$anchorScroll',
    'GROUP_ID',
    '$state'
  ];

  function JoinResultsCtrl(LoadResults,
                           $scope,
                           $anchorScroll,
                           GROUP_ID,
                           $state
  ) {
    var vm = this;

    vm.results = LoadResults.getResults();
    vm.currentPage = 1;
    vm.numPerPage = 6;
    vm.noResultsHelp = noResultsHelp;
    vm.pending = true;
    vm.startOver = startOver;

    $scope.$watch('result.currentPage + result.numPerPage', function() {
      var begin = ((vm.currentPage - 1) * vm.numPerPage);
      var end = begin + vm.numPerPage;
      vm.filteredResults = vm.results.slice(begin, end);
      $anchorScroll();
    });

    function noResultsHelp() {
      $state.go('group_finder.invitation', {groupId: GROUP_ID.NO_GROUP});
    }

    function startOver() {
      LoadResults.clearData();
      $state.go('group_finder.join.questions');
    }
  }
})();
