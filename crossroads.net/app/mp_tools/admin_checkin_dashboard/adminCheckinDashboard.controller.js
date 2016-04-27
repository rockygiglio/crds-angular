(function() {
  'use strict';

  module.exports = AdminCheckinDashboardController;

  AdminCheckinDashboardController.$inject = ['AuthService', 'CRDS_TOOLS_CONSTANTS', 'AdminCheckinDashboardService', 'EventService'];

  function AdminCheckinDashboardController(AuthService, CRDS_TOOLS_CONSTANTS, AdminCheckinDashboardService, EventService) {
    var vm = this;
    vm.site = {id: undefined};
    vm.eventsReady = false;
    vm.eventsLoading = false;
    vm.event = {id: undefined};
    vm.events = [];
    vm.eventRooms = [];
    vm.loadEvents = loadEvents;
    vm.loadRooms = loadRooms;

    vm.allowAdminAccess = function() {
      return (AuthService.isAuthenticated() && AuthService.isAuthorized(CRDS_TOOLS_CONSTANTS.SECURITY_ROLES.KidsClubTools));
    };

    function loadRooms() {
      vm.roomsLoading = true;
      vm.eventRooms = [];

      AdminCheckinDashboardService.checkinDashboard.get({ eventId: event.id},
        function (data) {
          vm.eventRooms = data.rooms;
          vm.roomsLoading = false;
        }
      );
    }

    function loadEvents() {
      reset();

      EventService.eventsBySite.query({ site : vm.site.id, template: false }, function(data) {
        vm.events = data;
        vm.eventsLoading = false;
      });
    }

    function reset() {
      vm.eventsReady = true;
      vm.eventsLoading = true;
      vm.roomsLoading = false;
      vm.eventRooms = [];
    }
  }
})();
