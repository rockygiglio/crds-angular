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

    vm.responses = Results.data;

    vm.results = Results.data.groups;
    vm.currentPage = 1;
    vm.numPerPage = 6;
    vm.noResultsHelp = noResultsHelp;

    $scope.$watch('result.currentPage + result.numPerPage', function() {
      var begin = ((vm.currentPage - 1) * vm.numPerPage);
      var end = begin + vm.numPerPage;
      vm.filteredResults = vm.results.slice(begin, end);
      $anchorScroll();
    });


    // Left in place for debugging purposes. Will remove at a later date
    // TODO remove before deploy
    function getResponses() {
      if (Responses.data.completed_flow === true) {
        sessionStorage.setItem('participant', angular.toJson(Responses.data));
      } else {
        Responses.data = angular.fromJson(sessionStorage.getItem('participant'));
      }

      return Responses.data;
    }

    function noResultsHelp() {
      $state.go('group_finder.invitation', {groupId: GROUP_ID.NO_GROUP});
    }
  }
})();
