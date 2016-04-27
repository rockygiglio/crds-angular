(function() {
  'use strict';

  module.exports = FormField;

  FormField.$inject = ['$templateRequest', '$compile'];

  function FormField($templateRequest, $compile) {
    return {
      restrict: 'E',
      scope: {
        field: '=?'
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

    function FormFieldController() {
      var vm = this;

    }

    function getTemplateUrl(className) {
      switch (className) {
        case 'EditableCheckbox':
          return 'templates/editableCheckbox.html';
        case 'EditableCheckboxGroupField':
          return 'templates/editableCheckboxGroupField.html';
        case 'EditableNumericField':
          return 'templates/editableNumericField.html';
        case 'EditableTextField':
          return 'templates/editableTextField.html';
        case 'EditableRadioField':
          return 'templates/editableRadioField.html';
        case 'EditableFormStep':
          return null;
        default:
          return 'templates/defaultField.html';
      }
    }
  }

})();
