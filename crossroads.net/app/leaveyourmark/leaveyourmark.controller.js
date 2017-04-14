(function() {
    'use strict';

    module.exports = LeaveYourMarkController;

    LeaveYourMarkController.$inject = [
        '$rootScope',
        '$filter',
        '$log',
        '$state',
        'LeaveYourMark'
    ];

    function LeaveYourMarkController(
        $rootScope,
        $filter,
        $log,
        $state,
        LeaveYourMark
    ) {
        var vm = this;

        vm.currentDay = undefined;
        vm.totalDays = undefined;
        vm.given = undefined;
        vm.committed = undefined;
        vm.givenGercentage = undefined
        vm.notStartedPercent = undefined
        vm.onPacePercent = undefined
        vm.behindPercent = undefined
        vm.completedPercent = undefined
        vm.viewReady = false;

        activate();

        function activate() {
            LeaveYourMark.campaignSummary
                         .get({pledgeCampaignId: 1103})
                         .$promise
                         .then((data) => {
                            vm.viewReady = true;
                            vm.currentDay = data.currentDay;
                            vm.totalDays = data.totalDays;
                            vm.given = data.totalGiven;
                            vm.committed = data.totalCommitted;
                            vm.givenGercentage = $filter('number')(vm.given / vm.committed * 100, 0);
                            vm.notStartedPercent = data.notStartedPercent;
                            vm.onPacePercent = data.onPacePercent;
                            vm.completedPercent = data.completedPercent;
                            vm.behindPercent = 100 - data.notStartedPercent - data.onPacePercent - data.completedPercent;
                         })
                         .catch((err) => {
                            vm.viewReady = true;
                            console.error(err);
                         });
        }
    }
})();
