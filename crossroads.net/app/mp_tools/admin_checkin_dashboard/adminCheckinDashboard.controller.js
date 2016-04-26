(function() {
  'use strict';

  module.exports = AdminCheckinDashboardController;

  AdminCheckinDashboardController.$inject = ['$log', 'AuthService', 'CRDS_TOOLS_CONSTANTS', 'AdminCheckinDashboardService'];

  function AdminCheckinDashboardController($log, AuthService, CRDS_TOOLS_CONSTANTS, AdminCheckinDashboardService) {
    var vm = this;
    vm.viewReady = false;
    vm.eventRooms = [];

    activate();

    function activate() {
        AdminCheckinDashboardService.checkinDashboard.get({ eventId: 4515378},
          function (data) {
            debugger;
            vm.eventRooms = data.rooms;
            vm.viewReady = true;
          },
          function (error) {
            setErrorState(error);
        });
    }

    vm.allowAdminAccess = function() {
      return (AuthService.isAuthenticated() && AuthService.isAuthorized(CRDS_TOOLS_CONSTANTS.SECURITY_ROLES.KidsClubTools));
    };
  }
})();
