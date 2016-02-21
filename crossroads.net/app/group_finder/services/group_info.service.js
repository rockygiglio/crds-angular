(function(){
  'use strict';

  module.exports = GroupInfoService;

  GroupInfoService.$inject = ['$http', '$cookies', '$q'];

  function GroupInfoService($http, $cookies, $q) {
    var groupInfo = {};
    var groups = {
      hosting: [],
      participating: [],
      byId: {}
    };

    // return $http.get('/app/group_finder/data/user.group.json')
    var requestComplete = false;
    var dataPromise = $http.get('/app/group_finder/data/user.group.json');
    dataPromise.then(function(res) {
      var cid = $cookies.get('userId');
      if (cid) {
        _.each(res.data.groups, function(group, i, list) {
          groups.byId[group.id] = group;
          _.each(group.members, function(member, i, list) {

            if (member.groupRoleId === 22) {
              if (member.contactId === parseInt(cid)) {
                group.isHost = true;
                group.host = member;
                groups.hosting.push(group);
              } else {
                group.isHost = false;
                group.host = member;
                groups.participating.push(group);
              }
            }
          });
        });
      }
      return groups;
    })
    .finally(function() {
      requestComplete = true;
    });

    groupInfo.getHosting = function() {
      return groups.hosting;
    };

    groupInfo.getParticipating = function() {
      return groups.participating;
    };

    groupInfo.findHosting = function(id) {
      return _.find(groups.hosting, function(group) {
        return group.id === parseInt(id);
      });
    };

    groupInfo.findGroupById = function(id) {
      return dataPromise.then(function() {
        return groups.byId[id];
      });
    };

    return groupInfo;

  }

})();
