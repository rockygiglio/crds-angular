(function(){
  'use strict';

  module.exports = ResultsService;

  ResultsService.$inject = ['$http'];

  function ResultsService($http) {
    // TODO Refactor for $resource
    return $http.get('/app/group_finder/data/results.json');
  }

})();
