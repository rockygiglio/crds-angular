(function() {
  'use strict';

  module.exports = JoinResultsCtrl;

  JoinResultsCtrl.$inject = [
    'Results',
    '$scope',
    '$anchorScroll',
    'GROUP_ID',
    '$state',
    'Responses',
    'LookupDefinitions'
  ];

  function JoinResultsCtrl(Results,
                           $scope,
                           $anchorScroll,
                           GROUP_ID,
                           $state,
                           Responses,
                           LookupDefinitions
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

      var lookup = LookupDefinitions.lookup;

      var participant = {
        address: {
          addressLine1: Responses.data.location.street,
          city: Responses.data.location.city,
          state: Responses.data.location.state,
          zip: Responses.data.location.zip
        },
        singleAttributes: Responses.getSingleAttributes(),
        attributeTypes: Responses.getMultiAttributes(['date_time_week', 'date_time_weekend'])
      };
      vm.resultsPromise = Results.loadResults(participant)
        .then(function displayResults() {
          vm.results = Results.getResults();
          vm.loading = false;

          $scope.$watch('result.currentPage + result.numPerPage', function() {
            var begin = ((vm.currentPage - 1) * vm.numPerPage);
            var end = begin + vm.numPerPage;
            vm.filteredResults = vm.results.slice(begin, end);
            $anchorScroll();
          });

        })
        .catch(function(error) {
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
