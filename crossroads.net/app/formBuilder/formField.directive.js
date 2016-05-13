(function() {
  'use strict';

  module.exports = FormField;

  FormField.$inject = ['$templateRequest', '$compile', 'Lookup'];

  function FormField($templateRequest, $compile) {
    return {
      restrict: 'E',
      scope: {
        field: '=?',
        responses: '=?'
      },
      link: function(scope, element) {
        var templateUrl = getTemplateUrl(scope.formField.field.className);
        if (templateUrl == null) {
          return;
        }

        $templateRequest(templateUrl).then(function(html) {
          var template = angular.element(html);
          element.append(template);
          $compile(template)(scope);
        });
      },
      controller: FormFieldController,
      controllerAs: 'formField',
      bindToController: true
    };

    function FormFieldController(Lookup) {
      var vm = this;
      vm.openBirthdatePicker = openBirthdatePicker;
      vm.crossroadsLocations = [];

      Lookup.query({ table: 'crossroadslocations' }, function(locations) {
        vm.crossroadsLocations = locations;
        vm.crossroadsLocations.splice(2, 1);
      });

      // TODO: See if moving the radiobutton specific code to another directive is better than this
      if (vm.field && vm.field.attributeType) {
        vm.attributeType = vm.field.attributeType;

        vm.singleAttributes = _.map(vm.attributeType.attributes, function(attribute) {
          var singleAttribute = {};
          singleAttribute[vm.attributeType.attributeTypeId] = {attribute: attribute};
          return singleAttribute;
        });
      }
    }

    function getTemplateUrl(className) {
      switch (className) {
        case 'EditableBooleanField':
          return 'templates/editableBooleanField.html';
        case 'EditableCheckbox':
          return 'templates/editableCheckbox.html';
        case 'EditableCheckboxGroupField':
          return 'templates/editableCheckboxGroupField.html';
        case 'EditableDateField':
          return 'templates/editableDateField.html';
        case 'EditableDatetimeField':
          return 'templates/editableDatetimeField.html';
        case 'EditableDropdown':
          return 'templates/editableDropDownField.html';
        case 'EditableNumericField':
          return 'templates/editableNumericField.html';
        case 'EditableRadioField':
          return 'templates/editableRadioField.html';
        case 'EditableTextField':
          return 'templates/editableTextField.html';
        case 'TextFieldReadOnly':
          return 'templates/textFieldReadOnly.html';          
        case 'EditableFormStep':
          return null;
        default:
          return 'templates/defaultField.html';
      }
    }

    function openBirthdatePicker($event) {
      $event.preventDefault();
      $event.stopPropagation();
      this.birthdateOpen = !this.birthdateOpen;
    }

  }

})();
