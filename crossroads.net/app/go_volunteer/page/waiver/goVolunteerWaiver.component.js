(function() {
  'use strict';

  module.exports = GoVolunteerWaiver;

  GoVolunteerWaiver.$inject = ['$rootScope', 'GoVolunteerService', 'GoVolunteerDataService', '$log', '$state'];

  function GoVolunteerWaiver($rootScope, GoVolunteerService, GoVolunteerDataService, $log, $state) {
    return {
      restrict: 'E',
      scope: {
        onSubmit: '&'
      },
      bindToController: true,
      controller: GoVolunteerWaiverController,
      controllerAs: 'goWaiver',
      templateUrl: 'waiver/goVolunteerWaiver.template.html'
    };

    function GoVolunteerWaiverController() {
      var vm = this;
      vm.processing = false;
      vm.submit = submit;
      vm.waiver = null;

      activate();

      function activate() {
        if ($state.toParams.organization === 'archdiocese') {
          vm.waiver = $rootScope.MESSAGES.goLocalArchdioceseWaiver.content;
        } else {
          vm.waiver = $rootScope.MESSAGES.goLocalWaiver.content;
        }
      }

      function submit() {
        if (vm.processing) {
          return false;
        }

        vm.processing = true;
        GoVolunteerService.saveSuccessful = false;
        try {
          var dto = GoVolunteerService.getRegistrationDto();
          GoVolunteerDataService.Create.save(dto, function(result) {
            console.log('success');
            vm.processing = false;
            GoVolunteerService.saveSuccessful = true;
            vm.onSubmit({nextState: 'thank-you'});
          },

          function(err) {
            $log.error(err);
            vm.processing = false;
            vm.onSubmit({nextState: 'thank-you'});
          });
        } catch (err) {
          console.log(err);
          vm.onSubmit({nextState: 'thank-you'});
        }
      }
    }
  }

})();
