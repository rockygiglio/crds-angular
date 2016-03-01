(function() {
  'use strict';

  module.exports = GroupDetailCtrl;

  GroupDetailCtrl.$inject = ['$scope', '$stateParams', '$modal', 'GroupInfo', 'GROUP_ROLE_ID_PARTICIPANT'];

  function GroupDetailCtrl($scope, $stateParams, $modal, GroupInfo, GROUP_ROLE_ID_PARTICIPANT) {
    var vm = this;

    vm.GROUP_ROLE_ID_PARTICIPANT = GROUP_ROLE_ID_PARTICIPANT;
    vm.group = GroupInfo.findHosting($stateParams.groupId);

    vm.emailGroup = emailGroup;

    $scope.$on('$viewContentLoaded', viewContentLoaded);

    //
    // Implementation
    //

    function viewContentLoaded(event){
      $scope.$parent.setGroup(vm.group);
    }

    function emailGroup() {
      var modalInstance = $modal.open({
        templateUrl: 'templates/group_contact_modal.html',
        controller: 'GroupContactCtrl as contactModal',
        resolve: {
          fromContactId: function() {
            return vm.group.contactId;
          },
          toContactIds: function() {
            return _.map(vm.group.members, function(member) {return member.contactId;});
          }
        }
      });

      modalInstance.result.then(function (selectedItem) {
        $scope.selected = selectedItem;
      });
    }

  }

})();
