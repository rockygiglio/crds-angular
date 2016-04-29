(function() {

  'use strict()';

  module.exports = EventSetupController;

  EventSetupController.$inject = [
      '$rootScope',
      'AuthService',
      'EventService',
      'CRDS_TOOLS_CONSTANTS',
  ];

  function EventSetupController($rootScope, AuthService, EventService, CRDS_TOOLS_CONSTANTS) {

    var vm = this;
    vm.viewReady = false;
    vm.site = {id: undefined};
    vm.template = {id: undefined};
    vm.event = {id: undefined};
    vm.eventTemplates = undefined;
    vm.events = undefined;
    vm.setup = setup;
    vm.loadEvents = loadEvents;

    vm.allowAdminAccess = function() {
      return (AuthService.isAuthenticated() && AuthService.isAuthorized(CRDS_TOOLS_CONSTANTS.SECURITY_ROLES.KidsClubTools));
    };

    ////////////////////////////////////////////

    function loadEvents() {
      // load templates first
      EventService.eventsBySite.query({ site: vm.site.id, template: true }, function(data) {
        vm.eventTemplates = data;
      });

      // load events
      EventService.eventsBySite.query({ site: vm.site.id, template: false }, function(data) {
        vm.events = data;
      });
    }

    function setup() {
      EventService.eventSetup.save({eventtemplateid: vm.template.id, eventid: vm.event.id},
        function(response) {
          //$rootScope.$emit('notify', $rootScope.MESSAGES.);
          vm.saving = false;
        },

        function(error) {
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
          vm.saving = false;
        });
    }
  }
})();
