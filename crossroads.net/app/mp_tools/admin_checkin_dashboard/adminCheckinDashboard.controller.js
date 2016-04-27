(function() {
  'use strict';

  module.exports = AdminCheckinDashboardController;

  AdminCheckinDashboardController.$inject = ['$log', '$scope', 'AuthService', 'CRDS_TOOLS_CONSTANTS', 'AdminCheckinDashboardService'];

  function AdminCheckinDashboardController($log, $scope, AuthService, CRDS_TOOLS_CONSTANTS, AdminCheckinDashboardService) {
    var vm = this;
    vm.site = {id: undefined};
    vm.event = {id: undefined};
    vm.events = [];
    vm.viewReady = false;
    vm.eventRooms = [];
    vm.loadEvents = loadEvents;

    activate();

    function activate() {
      AdminCheckinDashboardService.checkinDashboard.get({ eventId: 4515378},
        function (data) {
          vm.eventRooms = data.rooms;
          vm.viewReady = true;
        },
        function (error) {
          setErrorState(error);
      });
    }

    function loadEvents() {
      EventService.eventsBySite.query({ site : vm.site.id, template: false }, function(data) {
        vm.events = data;
      });
    }

    vm.allowAdminAccess = function() {
      return (AuthService.isAuthenticated() && AuthService.isAuthorized(CRDS_TOOLS_CONSTANTS.SECURITY_ROLES.KidsClubTools));
    };
  }
})();
