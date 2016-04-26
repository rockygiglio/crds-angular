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
      vm.eventRooms = [
        {name: 'KC101', label: '0-2 year olds', checkinAllowed: true, capacity: 21, assigned: 10},
        {name: 'KC201', label: '11-12 year olds', checkinAllowed: false, capacity: 21, assigned: 10},
        {name: 'KC301', label: '3-4 year olds', checkinAllowed: true, capacity: 21, assigned: 20},
      ]
      vm.viewReady = true;
    }

    vm.allowAdminAccess = function() {
      return (AuthService.isAuthenticated() && AuthService.isAuthorized(CRDS_TOOLS_CONSTANTS.SECURITY_ROLES.KidsClubTools));
    };
  }
})();
