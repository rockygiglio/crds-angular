(function () {
  'use strict';

  module.exports = GroupInvitationCtrl;

  GroupInvitationCtrl.$inject = [
    '$stateParams',
    'GroupInvitationService',
    'Responses'
  ];

  function GroupInvitationCtrl ($stateParams,
                                GroupInvitationService,
                                Responses) {

    var vm = this;

    vm.requestPending = true;
    vm.showInvite = false;
    vm.capacity = 0;

    // if there are responses, then the user came through QA flow
    if (_.has(Responses.data , 'completedQa')) {
      vm.capacity = 1;

      // Set capacity to account for invited spouse
      if (parseInt(Responses.data.relationship_status) === 2) {
        vm.capacity = 2;
        vm.showInvite = true;
      }
    }

    var promise = GroupInvitationService.acceptInvitation($stateParams.groupId, vm.capacity);
    promise.then(function() {
      // Invitation acceptance was successful
      vm.accepted = true;
    }, function(error) {
      // An error happened accepting the invitation
      vm.rejected = true;
    }).finally(function() {
      vm.requestPending = false;
    });
  }
})();
