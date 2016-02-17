(function() {
  'use strict';

  module.exports = GroupInvitationDirective;

  require('./group_invitation.html');

  GroupInvitationDirective.$inject = ['$log'];

  function GroupInvitationDirective($log) {
    return {
      restrict: 'AE',
      scope: {
        group: '='
      },
      controller: require('./group_invitation.controller'),
      controllerAs: 'ctrl',
      templateUrl: 'group_invitation/group_invitation.html'
    };
  }
})();
