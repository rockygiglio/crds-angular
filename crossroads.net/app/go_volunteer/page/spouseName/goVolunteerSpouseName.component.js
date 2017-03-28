(function() {
  'use strict';

  var moment = require('moment');

  module.exports = GoVolunteerSpouseName;

  GoVolunteerSpouseName.$inject = ['GoVolunteerService', 'Validation', '$rootScope'];

  function GoVolunteerSpouseName(GoVolunteerService, Validation, $rootScope) {
    return {
      restrict: 'E',
      scope: {
        onSubmit: '&'
      },
      bindToController: true,
      controller: GoVolunteerSpouseNameController,
      controllerAs: 'goSpouseName',
      templateUrl: 'spouseName/goVolunteerSpouseName.template.html'
    };

    function GoVolunteerSpouseNameController() {
      var now = new Date();

      var vm = this;

      vm.birthdateOpen = false;
      vm.initDate = new Date(now.getFullYear(), now.getMonth(), now.getDate());
      vm.maxBirthdate = new Date(now.getFullYear() - 18, now.getMonth(), now.getDate());
      vm.oneHundredFiftyYearsAgo = new Date(now.getFullYear() - 150, now.getMonth(), now.getDate());
      vm.openBirthdatePicker = openBirthdatePicker;
      vm.spouse = GoVolunteerService.spouse;
      vm.phoneFormat = /^\d{3}-\d{3}-\d{4}$/;
      vm.submit = submit;
      vm.validate = validate;
      vm.insertDashes = insertDashes;

      function openBirthdatePicker($event) {
        $event.preventDefault();
        $event.stopPropagation();

        vm.birthdateOpen = true;
      }

      function submit() {
        vm.spouseForm.$setSubmitted();
        if (vm.spouseForm.$valid) {
          vm.onSubmit({nextState: 'children'});
        } else {
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        }
      }

      function validate(fieldName) {
        return Validation.showErrors(vm.spouseForm, fieldName);
      }

      function insertDashes(e) {
        const { value } = e.target;
        const first3Re = /^\d{3}$/;
        const second3Re = /^\d{3}-\d{3}$/;

        let newValue = value;

        if (first3Re.test(value) || second3Re.test(value)) {
          newValue = value + '-';

          this.spouseForm.phone.$viewValue = newValue;
          this.spouseForm.phone.$render();
        }
      }
    }
  }
})();
