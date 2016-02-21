(function () {
  'use strict';

  module.exports = GroupInvitationCtrl;

  GroupInvitationCtrl.$inject = [
    '$cookies',
    '$stateParams',
    'GroupInfo',
    'GroupInvitationService',
    'Responses'
  ];

  function GroupInvitationCtrl ($cookies,
                                $stateParams,
                                GroupInfo,
                                GroupInvitationService,
                                Responses) {

    var vm = this;

    vm.requestPending = true;
    vm.contactId = $cookies.get('userId');
    vm.group = null;
    vm.showInvite = Responses.data.relationship_status = 2;
    //vm.showInvite = true;

    GroupInfo.findGroupById($stateParams.groupId)
      .then(function(group) {
        vm.group = group;
        if (group) {
          var promise = GroupInvitationService.acceptInvitation($stateParams.groupId, vm.contactId);
          promise.then(function() {
            // Invitation acceptance was successful
            vm.accepted = true;
          }, function(error) {
            // An error happened accepting the invitation
            vm.accepted = false;
          }).finally(function() {
            vm.requestPending = false;
          });
        }
      }, function(error) {
        vm.error = error;
      });

  }
})();
