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
      vm.childTwoSeven = 0;
      vm.childEightTwelve = 0;
      vm.childThirteenEighteen = 0;
      vm.submit = submit;
      vm.totalChildren = totalChildren;

      function submit() {
        GoVolunteerService.childrenAttending.childTwoSeven = vm.childTwoSeven;
        GoVolunteerService.childrenAttending.childEightTwelve = vm.childEightTwelve;
        GoVolunteerService.childrenAttending.childThirteenEighteen = vm.childThirteenEighteen;
        vm.onSubmit({nextState: 'group-connector'});
      }

      function totalChildren() {
        return vm.childTwoSeven + vm.childEightTwelve + vm.childThirteenEighteen;
      }
    }
  }

})();
