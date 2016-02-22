(function(){
  'use strict';

  module.exports = GroupInvitationService;

  GroupInvitationService.$inject = ['Group'];

  function GroupInvitationService(Group) {
    var service = {};
    service.acceptInvitation = acceptInvitation;

    function acceptInvitation(groupId, capacity) {
      var data = {
        'childCareNeeded': false,
        'groupRoleId': 16,
        'capacityNeeded': capacity,
        'sendConfirmationEmail': false
      };
      return Group.Participant.save({
        groupId: groupId
      }, [data]).$promise;
    }

    return service;
  }

})();
