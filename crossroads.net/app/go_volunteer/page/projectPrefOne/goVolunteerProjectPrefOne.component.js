(function() {
  'use strict';

  module.exports = GoVolunteerProjectPrefOne;

  GoVolunteerProjectPrefOne.$inject = ['GoVolunteerService'];

  function GoVolunteerProjectPrefOne(GoVolunteerService) {
    return {
      restrict: 'E',
      scope: {
        onSubmit: '&'
      },
      bindToController: true,
      controller: GoVolunteerProjectPrefOneController,
      controllerAs: 'goProjectPrefOne',
      templateUrl: 'projectPrefOne/goVolunteerProjectPrefOne.template.html'
    };

    function GoVolunteerProjectPrefOneController() {
      var vm = this;
      vm.projectTypes = GoVolunteerService.projectTypes;
      vm.submit = submit;

      function submit(projectTypeId) {
        GoVolunteerService.projectPrefOne = projectTypeId;
        vm.onSubmit({nextState: 'project-preference-two'});
      }

    }
  }

})();
