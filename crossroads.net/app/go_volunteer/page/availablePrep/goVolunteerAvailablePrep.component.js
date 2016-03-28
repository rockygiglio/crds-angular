(function() {
  'use strict';

  module.exports = GoVolunteerAvailablePrep;

  GoVolunteerAvailablePrep.$inject = ['GoVolunteerService'];

  function GoVolunteerAvailablePrep(GoVolunteerService) {
    return {
      restrict: 'E',
      scope: {
        onSubmit: '&' 
      },
      bindToController: true,
      controller: GoVolunteerAvailablePrepController,
      controllerAs: 'goAvailablePrep',
      templateUrl: 'availablePrep/goVolunteerAvailablePrep.template.html'
    };

    function GoVolunteerAvailablePrepController() {
      var vm = this;
      vm.chooseTime = chooseTime;
      vm.prepWork = GoVolunteerService.prepWork;

      activate();
      /////////////////////////

      function activate() {
        if (vm.prepWork === undefined || _.isEmpty(vm.prepWork)) {
          // set the availability to no?

          vm.onSubmit({nextState: 'waiver'});
        }
      }

      function chooseTime(prepTime) {
        GoVolunteerService.myPrepTime = prepTime;
        vm.onSubmit({nextState: 'available-prep-spouse'});
      }
    }
  }

})();
