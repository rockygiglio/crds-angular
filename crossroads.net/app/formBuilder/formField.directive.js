(function() {
  'use strict';

  module.exports = FormField;

  FormField.$inject = ['$templateRequest', '$compile'];

  function FormField($templateRequest, $compile) {
    return {
      restrict: 'E',
      scope: {
        field: '=?',
        responses: '=?'
      },
      link: function(scope, element) {
        var templateUrl = getTemplateUrl(scope.formField.field);
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

    function getTemplateUrl(field) {
      switch (field.className) {
        case 'EditableBooleanField':
          return 'templates/editableBooleanField.html';
        case 'EditableCheckbox':
          return 'templates/editableCheckbox.html';
        case 'EditableCheckboxGroupField':
          return 'templates/editableCheckboxGroupField.html';
        case 'EditableNumericField':
          return 'templates/editableNumericField.html';
        case 'EditableRadioField':
          return 'templates/editableRadioField.html';
        case 'EditableTextField':
          return 'templates/editableTextField.html';
        case 'ProfileField':
          return getProfileTemplateUrl(field);
        case 'EditableFormStep':
          return null;
        default:
          return 'templates/defaultField.html';
      }
    }

    function getProfileTemplateUrl(field) {
      //TODO: See if we can simplify / possibly strategy pattern
      switch(field.mPField) {
        case 'Email':
          return 'templates/profileEmail.html';
        case 'Ethnicity':
          return 'templates/profileEthnicity.html';
        case 'Gender':
          return 'templates/profileGender.html';
        case 'Name':
          return 'templates/profileName.html';
        default:
          return 'templates/defaultField.html';
      }
    }
  }

})();
