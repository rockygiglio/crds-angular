(function() {
  'use strict';

  module.exports = AdminCheckinDashboardController;

  AdminCheckinDashboardController.$inject = ['$log', 'AuthService', 'CRDS_TOOLS_CONSTANTS'];

  function AdminCheckinDashboardController($log, AuthService, CRDS_TOOLS_CONSTANTS) {
    var vm = this;
    vm.viewReady = false;
    vm.eventRooms

    activate();

    function activate() {
    }

    vm.allowAdminAccess = function() {
      return (AuthService.isAuthenticated() && AuthService.isAuthorized(CRDS_TOOLS_CONSTANTS.SECURITY_ROLES.KidsClubTools));
    };
  }
})();
