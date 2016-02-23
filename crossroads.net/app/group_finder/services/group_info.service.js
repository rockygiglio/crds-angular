(function(){
  'use strict';

  module.exports = GroupInfoService;

  GroupInfoService.$inject = ['$cookies', 'Group', 'GROUP_API_CONSTANTS', 'AUTH_EVENTS', '$rootScope'];

  function GroupInfoService($cookies, Group, GROUP_API_CONSTANTS, AUTH_EVENTS, $rootScope) {
    var groupInfo = {};
    var groups = {
      hosting: [],
      participating: []
    };

    function clearData() {
      groups.hosting = [];
      groups.participating = [];
    }

    $rootScope.$on(AUTH_EVENTS.logoutSuccess, clearData);

    var requestComplete = false;
    var GroupType = Group.Type.query({groupTypeId: GROUP_API_CONSTANTS.GROUP_TYPE_ID}, function(data) {
      var cid = $cookies.get('userId');
      if (cid) {
        _.each(data, function(group) {
          if (group.contactId === parseInt(cid)) {
            group.isHost = true;
            groups.hosting.push(group);
          } else {
            group.isHost = false;
            groups.participating.push(group);
          }
        });
      }
      requestComplete = true;
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
