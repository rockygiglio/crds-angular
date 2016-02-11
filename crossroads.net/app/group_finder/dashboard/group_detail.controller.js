(function() {
  'use strict';

  module.exports = GroupDetailCtrl;

  GroupDetailCtrl.$inject = ['$log', '$scope', 'GroupInfo', '$stateParams', '$modal'];

  function GroupDetailCtrl($log, $scope, GroupInfo, $stateParams, $modal) {
    var vm = this;

    vm.group = GroupInfo.findHosting($stateParams.groupId);

    vm.tabs = [
      { title:'Host Resources', active: false, route: 'dashboard.resources' },
      { title:'My Group', active: true, route: 'dashboard.group'},
    ];

    vm.emailGroup = function() {
      // TODO popup with text block?
      $log.debug('Sending Email to group');
      var modalInstance = $modal.open({
        templateUrl: 'templates/group_contact_modal.html',
        controller: 'GroupContactCtrl as contactModal',
        resolve: {
          fromContactId: function() {
            return vm.group.host.contactId;
          },
          toContactIds: function() {
            return _.map(vm.group.members, function(member) {return member.contactId;});
          }
        }
      });

      modalInstance.result.then(function (selectedItem) {
        $scope.selected = selectedItem;
      }, function () {
        $log.info('Modal dismissed at: ' + new Date());
      });
    };

    vm.inviteMember = function(email) {
      // TODO add validation. Review how to send email without `toContactId`
      $log.debug('Sending Email to: ' + email);
      var toSend = {
        'fromContactId': vm.group.host.contactId,
        'fromUserId': 0,
        'toContactId': 0,
        'templateId': 0,
        'mergeData': {}
      };

    };
  }

})();
