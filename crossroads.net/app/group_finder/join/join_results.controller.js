(function() {
  'use strict';

  module.exports = JoinResultsCtrl;

  JoinResultsCtrl.$inject = [
    'Results',
    '$scope',
    '$anchorScroll',
    'GROUP_ID',
    '$state',
    'Responses'
  ];

  function JoinResultsCtrl(Results,
                           $scope,
                           $anchorScroll,
                           GROUP_ID,
                           $state,
                           Responses
  ) {
    var vm = this;

    vm.loading = true;
    vm.currentPage = 1;
    vm.numPerPage = 6;
    vm.noResultsHelp = noResultsHelp;
    vm.pending = true;
    vm.startOver = startOver;
    vm.initialize = initialize;
    vm.error = false;

    function initialize() {

      if (!Responses.data.completed_flow) {
        $state.go('group_finder.join.questions');
      }

      Results.clearData();

      var participant = {
        singleAttributes: Responses.getSingleAttributes(),
        attributeTypes: Responses.getMultiAttributes(['date_time_week', 'date_time_weekend'])
      };
      vm.resultsPromise = Results.loadResults(participant)
        .then(function displayResults(value) {
          vm.results = Results.getResults();
          vm.loading = false;

          $scope.$watch('result.currentPage + result.numPerPage', function() {
            var begin = ((vm.currentPage - 1) * vm.numPerPage);
            var end = begin + vm.numPerPage;
            vm.filteredResults = vm.results.slice(begin, end);
            $anchorScroll();
          });

        })
        .catch(function error(error) {
          console.log('Search Result Error:', error);
          vm.error = true;
          vm.loading = false;
        });
    }

    function noResultsHelp() {
      $state.go('group_finder.invitation', {groupId: GROUP_ID.NO_GROUP});
    }

    function startOver() {
      Results.clearData();
      $state.go('group_finder.join.questions');
    }

    vm.initialize();
  }
})();
