(function() {
  'use strict';

  module.exports = JoinResultsCtrl;

  JoinResultsCtrl.$inject = [
    'Results',
    'Responses',
    '$scope',
    '$anchorScroll',
    'GROUP_ID',
    '$state'
  ];

  function JoinResultsCtrl(Results,
                           Responses,
                           $scope,
                           $anchorScroll,
                           GROUP_ID,
                           $state
  ) {
    var vm = this;

    vm.results = Results.data.groups.slice(0,12);
    vm.currentPage = 1;
    vm.numPerPage = 6;
    vm.noResultsHelp = noResultsHelp;

    $scope.$watch('result.currentPage + result.numPerPage', function() {
      var begin = ((vm.currentPage - 1) * vm.numPerPage);
      var end = begin + vm.numPerPage;
      vm.filteredResults = vm.results.slice(begin, end);
      $anchorScroll();
    });

    function noResultsHelp() {
      $state.go('group_finder.invitation', {groupId: GROUP_ID.NO_GROUP});
    }
  }
})();
