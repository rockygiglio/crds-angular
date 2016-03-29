(function() {
  'use strict';

  module.exports = GoVolunteerProjectPrefThree;

  GoVolunteerProjectPrefThree.$inject = ['GoVolunteerService'];

  function GoVolunteerProjectPrefThree(GoVolunteerService) {
    return {
      restrict: 'E',
      scope: {
        onSubmit: '&'
      },
      bindToController: true,
      controller: GoVolunteerProjectPrefThreeController,
      controllerAs: 'goProjectPrefThree',
      templateUrl: 'projectPrefThree/goVolunteerProjectPrefThree.template.html'
    };

    function GoVolunteerProjectPrefThreeController($sce) {
      var vm = this;

      vm.projectTypes = GoVolunteerService.projectTypes;
      vm.alreadySelected = alreadySelected;
      vm.submit = submit;

      function alreadySelected(projectTypeId) {
        if ((GoVolunteerService.projectPrefOne === projectTypeId) || (GoVolunteerService.projectPrefTwo === projectTypeId)) {
          return ['disabled', 'checked'];
        }

        return [];
      }

      function submit(projectTypeId) {
        if ((GoVolunteerService.projectPrefOne === projectTypeId) || (GoVolunteerService.projectPrefTwo === projectTypeId)) {
          return;
        }

        GoVolunteerService.projectPrefThree = projectTypeId;
        vm.onSubmit({nextState: 'unique-skills'});
      }

    }
  }

})();
