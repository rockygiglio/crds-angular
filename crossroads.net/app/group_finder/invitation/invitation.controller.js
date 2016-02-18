(function () {
  'use strict';

  module.exports = GroupInvitationCtrl;

  GroupInvitationCtrl.$inject = ['$cookies', '$stateParams', 'GroupInfo', 'GroupInvitationService', '$log'];

  function GroupInvitationCtrl ($cookies, $stateParams, GroupInfo, GroupInvitationService, $log) {
    $log.debug("GroupInvitationCtrl");

    var vm = this;

    vm.requestPending = true;
    vm.contactId = $cookies.get('userId');
    vm.group = null;

    GroupInfo.findGroupById($stateParams.groupId)
      .then(function(group) {
        vm.group = group;
        if (group) {
          // TODO - Do we need to validate the this particular user was invited to the group?
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
