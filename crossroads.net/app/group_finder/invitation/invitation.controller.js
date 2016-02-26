(function () {
  'use strict';

  module.exports = GroupInvitationCtrl;

  GroupInvitationCtrl.$inject = [
    '$stateParams',
    'GroupInvitationService',
    'Responses',
    'GROUP_ROLE_ID_PARTICIPANT',
    '$state',
    '$rootScope',
    'GroupInfo'
  ];

  function GroupInvitationCtrl ($stateParams,
                                GroupInvitationService,
                                Responses,
                                GROUP_ROLE_ID_PARTICIPANT,
                                $state,
                                $rootScope,
                                GroupInfo) {

    var vm = this;

    vm.groupId = $stateParams.groupId;
    vm.requestPending = true;
    vm.showInvite = false;
    vm.capacity = 0;
    vm.goToDashboard = goToDashboard;
    vm.initialize = initialize;
    vm.groupId = parseInt($stateParams.groupId);
    vm.alreadyJoined = false;

    vm.groups = {
      hosting: GroupInfo.getHosting(),
      participating: GroupInfo.getParticipating()
    };

    _.each(vm.groups.participating, function(group) {
      if (group.groupId === vm.groupId)  {
        vm.requestPending = false;
        vm.alreadyJoined = true;
      }
    });

    // if there are responses, then the user came through QA flow
    function initialize() {
      if (vm.alreadyJoined === false) {
        if (_.has(Responses.data , 'completedQa')) {
          vm.capacity = 1;

          // Set capacity to account for invited spouse
          if (parseInt(Responses.data.relationship_status) === 2) {
            vm.capacity = 2;
            vm.showInvite = true;
          }
        }

        var promise = GroupInvitationService.acceptInvitation(vm.groupId,
          {capacity: vm.capacity, groupRoleId: GROUP_ROLE_ID_PARTICIPANT}
        );
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
    }

    function goToDashboard() {
      $rootScope.$broadcast('reloadGroups');
      $state.go('group_finder.dashboard');
    }

    vm.initialize();
  }
})();
