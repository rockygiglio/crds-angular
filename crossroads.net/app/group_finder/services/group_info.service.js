(function(){
  'use strict';

  module.exports = GroupInfoService;

  GroupInfoService.$inject = ['$http', '$cookies'];

  function GroupInfoService($http, $cookies) {
    var groupInfo = {};
    var groups = {
      hosting: [],
      participating: []
    };

    // return $http.get('/app/group_finder/data/user.group.json')
    $http.get('/app/group_finder/data/user.group.json')
      .then(function(res) {
        var cid = $cookies.get('userId');
        if (cid) {
          _.each(res.data.groups, function(group, i, list) {
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

    return groupInfo;

  }

})();
