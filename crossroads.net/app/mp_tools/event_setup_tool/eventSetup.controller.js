(function() {

    'use strict()';

    module.exports = EventSetupController;

    EventSetupController.$inject = [
        '$log',
        '$location',
        '$window',
        'MPTools',
        '$rootScope',
        'AuthService',
        'EventService',
        'CRDS_TOOLS_CONSTANTS',
        'LookupService'
    ];

    function EventSetupController($log, $location, $window, MPTools, $rootScope, AuthService, EventService, CRDS_TOOLS_CONSTANTS, LookupService) {


        //
        //vm.allParticipants = [];
        //vm.cancel = cancel;
        //vm.eventDates = [];
        //vm.format = 'MM/dd/yyyy';
        //vm.frequencies = [
        //    { value: 0, text: 'Once' },
        //    { value: 1, text: 'Every Week' },
        //    { value: 2, text: 'Every Other Week' }
        //];
        //vm.fromOpened = false;
        //vm.group = {};
        //vm.isFrequencyOnce = isFrequencyOnce;
        //vm.isFrequencyMoreThanOnce = isFrequencyMoreThanOnce;
        //vm.open = openDatePicker;

        //vm.populateDates = populateDates;
        //vm.saveRsvp = saveRsvp;
        //vm.showError = showError;
        //vm.ready = false;

        var vm = this;
        vm.allowAccess = allowAccess;
        vm.errorMessage = $rootScope.MESSAGES.toolsError;
        //vm.group = {};
        vm.multipleRecordsSelected = true;
        vm.params = MPTools.getParams();
        vm.showError = showError;
        vm.viewReady = false;

        vm.site = {id: undefined};
        vm.template = {id: undefined};
        vm.event = {id: undefined};
        vm.eventTemplates = undefined;
        vm.events = undefined;

        vm.setup = setup;

        vm.loadEvents = loadEvents;

        activate();

        ////////////////////////////////////////////

        function activate() {
          vm.multipleRecordsSelected = showError();
        }

        function loadEvents() {
          // load templates first
          EventService.eventsBySite.query({ site : vm.site.id, template: true }, function(data) {
              vm.eventTemplates = data;
          });

          // load events
          EventService.eventsBySite.query({ site : vm.site.id, template: false }, function(data) {
              vm.events = data;
          });
        }

        function allowAccess() {
            return (AuthService.isAuthenticated() && AuthService.isAuthorized(CRDS_TOOLS_CONSTANTS.SECURITY_ROLES.EventSetupTool));
        }

        function cancel() {
            $window.close();
        }

        function showError() {
            return vm.params.selectedCount > 1 || vm.params.recordDescription === undefined || vm.params.recordId === '-1';
        }

        function showError() {
            if (vm.params.selectedCount > 1 ||
                vm.params.recordDescription === undefined ||
                vm.params.recordId === '-1') {
                vm.viewReady = true;
                return true;
            }
            return false;
        }

        function setup() {
            debugger;
            EventService.eventSetup.save({eventtemplateid: vm.templateId, eventid: vm.eventId}).$promise.then(function(response) {
                //$rootScope.$emit('notify', $rootScope.MESSAGES.);
                vm.saving = false;
            }, function(error) {
                $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
                vm.saving = false;
            });
        }
    }

})();
