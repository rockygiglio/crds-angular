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

        vm.locations = {};
        vm.locationID = undefined;
        vm.loadEvents = loadEvents;

        //$scope.$watch(vm.locationID, loadEvents);

        activate();

        ////////////////////////////////////////////

        function activate() {
            vm.multipleRecordsSelected = showError();

            LookupService.Congregations.query({
             }, function(cong) {
                vm.locations = cong;
            });
        }

        function loadEvents() {
            debugger;
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
    }

})();
