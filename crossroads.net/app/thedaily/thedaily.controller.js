(function() {
    'use strict';

    module.exports = TheDailyController;

    TheDailyController.$inject = [
        '$rootScope',
        '$log',
        '$state',
        'EmailSubscriptionService',
        'Validation'
    ];

    function TheDailyController(
        $rootScope,
        $log,
        $state,
        EmailSubscriptionService,
        Validation
    ) {

        $log.debug('Inside The Daily Controller');

        var vm = this;
        vm.saving = false;
        vm.email = '';
        vm.listName = 'The Daily'; // need to pull from environment or build step
        vm.validation = Validation;
        vm.submitSignup = submitSignup;

        function submitSignup() {
            debugger;
            vm.saving = true;
            if (vm.MailchimpSubscriptionForm.$valid) {
                EmailSubscriptionService.SubscriptionSignup.save({ emailAddress: vm.email, listName: vm.listName }, function(response) {

                    if (response.ErrorInSignupProcess === true) {
                        debugger;
                        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
                    } else if (response.UserAlreadySubscribed === true) {
                        debugger;
                        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
                    } else if (response.UserAlreadySubscribed === false) {
                        debugger;
                        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
                    }

                    vm.saving = false;
                    debugger;

                }, function(error) {
                    $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
                    vm.saving = false;
                });
            } else {
                vm.saving = false;
            }

        }
    }
})();
