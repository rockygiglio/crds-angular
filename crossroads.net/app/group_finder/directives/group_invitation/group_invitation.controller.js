(function(){
  'use strict';

  module.exports = GroupInvitationCtrl;

  GroupInvitationCtrl.$inject = ['$scope', '$log'];

  function GroupInvitationCtrl($scope, $log) {
    var vm = this;
    vm.inviteMember = inviteMember;

    //
    // Controller implementation
    //

    function inviteMember() {
      // TODO add validation. Review how to send email without `toContactId`
      $log.debug('Sending group invitation Email to: ' + vm.invitee);
      var toSend = {
          'fromContactId': $scope.group.host.contactId,
          'fromUserId': 0,
          'toContactId': 0,
          'templateId': 0,
          'mergeData': {}
      };
    }
  }

})();
