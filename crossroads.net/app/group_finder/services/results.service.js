(function(){
  'use strict';

  module.exports = ResultsService;

  ResultsService.$inject = ['$http'];

  function ResultsService($http) {
    var results = {};
    var groups = [];

    $http.get('/app/group_finder/data/results.json')
      .then(function(res) {
        _.each(res.data.groups, function(group, i, list) {
          group.host = _.find(group.members, function(member) {
            return parseInt(member.groupRoleId) === 22;
          });
          groups.push(group);
        });
        return groups;
      });

    results.getGroups = function() {
      return groups;
    };

    return results;

  }

})();
