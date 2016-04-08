(function() {
  'use strict';

  module.exports = GoVolunteerWaiver;

  GoVolunteerWaiver.$inject = ['$rootScope', 'GoVolunteerService', 'GoVolunteerDataService', '$log'];

  function GoVolunteerWaiver($rootScope, GoVolunteerService, GoVolunteerDataService, $log) {
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
        if (GoVolunteerService.cmsInfo && GoVolunteerService.cmsInfo.pages.length > 0) {
          vm.waiver = GoVolunteerService.cmsInfo.pages[0].content;
        } else {
          vm.waiver = $rootScope.MESSAGES.goVolunteerWaiverTerms.content;
        }
      }

      function submit() {
        vm.processing = true;
        var dto = GoVolunteerService.getRegistrationDto();
        GoVolunteerDataService.Create.save(dto, function(result) {
          console.log('success');
          vm.onSubmit({nextState: 'thank-you'});
        },

        function(err) {
          $log.error(err);
          $rootScope.$emit('notify', $rootScope.MESSAGES.eventToolProblemSaving);
        });

        vm.processing = false;
      }
    }
  }

})();
