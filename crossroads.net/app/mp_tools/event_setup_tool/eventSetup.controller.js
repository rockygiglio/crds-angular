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
        'CRDS_TOOLS_CONSTANTS'
    ];

    function EventSetupController($log, $location, $window, MPTools, $rootScope, AuthService, EventService, CRDS_TOOLS_CONSTANTS) {


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



        activate();

        ////////////////////////////////////////////

        function activate() {
            vm.multipleRecordsSelected = showError();

            //function getDonations() {
            //    vm.donation_view_ready = false;
            //    GivingHistoryService.donations.get({ donationYear: vm.selected_giving_year.key,
            //            softCredit: false,
            //            impersonateDonorId: vm.impersonate_donor_id },
            //        function (data) {
            //            vm.donations = data.donations;
            //            vm.donation_total_amount = data.donation_total_amount;
            //            vm.donation_statement_total_amount = data.donation_statement_total_amount;
            //            vm.donation_view_ready = true;
            //            vm.donation_history = true;
            //            vm.donations_all = vm.selected_giving_year.key === '' ? true : false;
            //            vm.beginning_donation_date = data.beginning_donation_date;
            //            vm.ending_donation_date = data.ending_donation_date;
            //        },
            //
            //        function (error) {
            //            vm.donation_history = false;
            //            vm.donation_view_ready = true;
            //            setErrorState(error);
            //        });
            //}

            //EventService.eventsBySite.get({})

            //Su2sData.get({
            //    'oppId': vm.params.recordId
            //}, function(g) {
            //    vm.group = g;
            //    vm.allParticipants = g.Participants;
            //    vm.ready = true;
            //});
            //populateDates();
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
