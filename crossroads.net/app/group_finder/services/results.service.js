(function(){
  'use strict';

  module.exports = ResultsService;

  ResultsService.$inject = ['$http'];

  function ResultsService($http) {
    var results = {};
    var groups = [];

    $http.get('/app/group_finder/data/results.json')
      .then(function(res) {
        groups = res.data.groups;
        return groups;
      });

    results.getGroups = function() {
      return groups;
    };

    results.find = function(groupId) {
      var found = _.find(groups, function (group) {
        return group.id === parseInt(groupId);
      });

      return found;
    };

    return results;

  }

})();
