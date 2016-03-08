(function(){
  'use strict';

  module.exports = ResultsService;

  ResultsService.$inject = ['$http', 'Address'];

  function ResultsService($http, Address) {
    var requestPromise = null;
    var results = {};
    var groups = [];

    results.loadResults = loadResults;
    results.clearData = clearData;
    results.getResults = getResults;

    function loadResults() {
      if (!requestPromise) {
        requestPromise = $http.get('/app/group_finder/data/resultsByType.json');
        requestPromise.then(function(results) {
          clearData();

          groups = results.data.slice(0,12);

          _.each(groups, function(group) {
            group.time = displayTime(group.meetingDayId, group.meetingTime);
            group.description = group.groupDescription;
            group.id = group.groupId;
            group.groupTitle = groupTitle(group.contactName);
            group.mapLink = Address.mapLink(group.address);

            if (_.has(group.singleAttributes, '73' ) ) {
              group.groupType = group.singleAttributes[73].attribute.description;
            }
            group.attributes = [];

            //
            // check attributes for pets and kids
            //
            _.each(group.attributeTypes, function(attribute) {
              if (attribute.name === 'Pets') {
                _.each(attribute.attributes, function(type) {
                  if (type.selected) {
                    if (type.name.substr('dog')) { group.attributes.push('has a dog'); }
                    if (type.name.substr('cat')) { group.attributes.push('has a cat'); }
                  }
                });
              }
              if (attribute.name === 'Kids') {
                _.each(attribute.attributes, function(type) {
                  if (type.selected && type.name.substr('kid')) { group.attributes.push('kids welcome'); }
                });
              }
            });
          });

          console.log(groups);
        });
      }
    }

    function getGroupAttributes() {
      var ret = [];
      if (vm.lookupContains(vm.responses.kids, 'kid')) { ret.push('kids welcome'); }

      if (vm.responses.pets) {
        _.each(vm.responses.pets, function(value, id) {
          if (value) {
            if (vm.lookupContains(id, 'dog')) { ret.push('has a dog'); }
            if (vm.lookupContains(id, 'cat')) { ret.push('has a cat'); }
          }
        });
      }
      return ret;
    }

    function displayTime(day, time) {
      var _time = time.split(':');
      var format = 'dddd[s] @ h:mm a';
      if (parseInt(_time[1]) === 0) {
        format = 'dddd[s] @ h a';
      }
      return moment().isoWeekday(day - 1).hour(_time[0]).minute(_time[1]).format(format);
    }

    function groupTitle(name) {
      var _name = name.split(', ');
      return _name[1] + ' ' +  _name[0][0] + '.';
    }

    function clearData() {
      groups = [];
    }

    function getResults() {
      return groups;
    }

    return results;
  }

})();
