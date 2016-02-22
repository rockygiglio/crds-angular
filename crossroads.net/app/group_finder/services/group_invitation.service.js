(function(){
  'use strict';

  module.exports = GroupInvitationService;

  GroupInvitationService.$inject = ['$log', 'Group', 'Responses'];

  function GroupInvitationService($log, Group, Responses) {
    var service = {};
    service.acceptInvitation = acceptInvitation;

    function acceptInvitation(groupId) {

      // Default capacity to 0 for those who were invited
      var capacity = 0;

      // if there are responses, then the user came through QA flow
      if (_.has(Responses.data , 'completedQa')) {
        capacity = 1;

        // Set capacity to account for invited spouse
        if (parseInt(Responses.data.relationship_status) === 2) {
          capacity = 2;
        }
      }
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
