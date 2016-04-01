(function() {
  'use strict';

  module.exports = GoVolunteerChildrenCount;

  GoVolunteerChildrenCount.$inject = ['GoVolunteerService'];

  function GoVolunteerChildrenCount(GoVolunteerService) {
    return {
      restrict: 'E',
      scope: {
        onSubmit: '&'
      },
      bindToController: true,
      controller: GoVolunteerChildrenCountController,
      controllerAs: 'goChildrenCount',
      templateUrl: 'childrenCount/goVolunteerChildrenCount.template.html'
    };

    function GoVolunteerChildrenCountController() {
      var vm = this;
      vm.childrenAttending = GoVolunteerService.childrenAttending;
      vm.submit = submit;
      vm.totalChildren = totalChildren;

      function submit() {
        //GoVolunteerService.childrenAttending.childTwoSeven = vm.childTwoSeven;
        //GoVolunteerService.childrenAttending.childEightTwelve = vm.childEightTwelve;
        //GoVolunteerService.childrenAttending.childThirteenEighteen = vm.childThirteenEighteen;
        vm.onSubmit({nextState: 'group-connector'});
      }

      function totalChildren() {
        return vm.childrenAttending.childTwoSeven + vm.childrenAttending.childEightTwelve + vm.childrenAttending.childThirteenEighteen;
      }
    }
  }

})();
