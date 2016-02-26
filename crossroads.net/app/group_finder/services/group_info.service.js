(function(){
  'use strict';

  module.exports = GroupInfoService;

  GroupInfoService.$inject = ['$cookies', 'Group', 'GROUP_API_CONSTANTS', 'AUTH_EVENTS', '$rootScope'];

  function GroupInfoService($cookies, Group, GROUP_API_CONSTANTS, AUTH_EVENTS, $rootScope) {
    var requestPromise = null;

    //
    // Group Info service definition
    //
    var groupInfo = {};
    var groups = {
      hosting: [],
      participating: []
    };

    groupInfo.loadGroupInfo = loadGroupInfo;
    groupInfo.getHosting = getHosting;
    groupInfo.getParticipating = getParticipating;
    groupInfo.findHosting = findHosting;

    // Clear the group info cache when the user logs out
    $rootScope.$on(AUTH_EVENTS.logoutSuccess, clearData);

    //
    // Initialize the data
    //
    function loadGroupInfo() {
      if (!requestPromise) {
        requestPromise = Group.Type.query({groupTypeId: GROUP_API_CONSTANTS.GROUP_TYPE_ID}).$promise;
        requestPromise.then(function(data) {
          var cid = $cookies.get('userId');
          if (cid) {
            _.each(data, function(group) {
              if (group.contactId === parseInt(cid)) {
                group.isHost = true;
                groups.hosting.push(group);

                // Query the other participants of the group
                queryParticipants(group);
              } else {
                group.isHost = false;
                groups.participating.push(group);
              }

              // Determine if group is private
              if (!group.meetingTime || !group.meetingDayId || !group.address) {
                group.isPrivate = true;
              }
            });
          }
          return groups;
        }, function error() {
          // An error occurred, clear the promise so another attempt can be made
          requestPromise = null;
        });
      }

      return requestPromise;
    }

    //
    // Service implementation
    //

    function getHosting() {
      return groups.hosting;
    }

    function getParticipating() {
      return groups.participating;
    }

    function findHosting(id) {
      return _.find(groups.hosting, function(group) {
        return group.groupId === parseInt(id);
      });
    }

    function queryParticipants(group) {
      Group.Participant.query({ groupId: group.groupId }).$promise.then(function(data) {
        var members = [];

        _.each(data, function(person) {
          members.push({
            contactId: person.contactId,
            participantId: person.participantId,
            groupRoleId: person.groupRoleId,
            groupRoleTitle: person.groupRoleTitle,
            emailAddress: person.email,
            firstName: person.nickName,
            lastName: person.lastName,
            affinities: person.attributes
          });
        });

        group.members = members;
      });
    }

    function clearData() {
      requestPromise = null;
      groups.hosting = [];
      groups.participating = [];
    }

    $rootScope.$on('reloadGroups', clearData);

    //
    // Return the service
    //

    return groupInfo;
  }

})();
