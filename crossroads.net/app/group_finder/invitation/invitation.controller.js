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
    if (_.has(Responses.data, 'relationship_status')) {
      vm.showInvite = Responses.data.relationship_status === 2;
    }

    var promise = GroupInvitationService.acceptInvitation($stateParams.groupId);
    promise.then(function() {
      // Invitation acceptance was successful
      vm.accepted = true;
    }, function(error) {
      // An error happened accepting the invitation
      vm.rejected = true
      ;
    }).finally(function() {
      vm.requestPending = false;
    });
  }
})();
