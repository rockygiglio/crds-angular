(function() {
  'use strict';

  module.exports = FormField;

  FormField.$inject = ['$templateRequest', '$compile'];

  function FormField($templateRequest, $compile) {
    return {
      restrict: 'E',
      scope: {
        field: '=?',
        data: '=?'
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
          return 'default/editableBooleanField.html';
        case 'EditableCheckbox':
          return 'default/editableCheckbox.html';
        case 'EditableCheckboxGroupField':
          return 'default/editableCheckboxGroupField.html';
        case 'EditableNumericField':
          return 'default/editableNumericField.html';
        case 'EditableRadioField':
          return 'default/editableRadioField.html';
        case 'EditableTextField':
          return 'default/editableTextField.html';
        case 'ProfileField':
        case 'GroupParticipantField':
          return getMPTemplateUrl(field);
        case 'EditableFormStep':
          return null;
        default:
          return 'default/defaultField.html';
      }
    }

    function getMPTemplateUrl(field) {
      //TODO: See if we can simplify / possibly strategy pattern
      switch(field.mPField) {
        case 'Childcare':
          return 'groupParticipant/childcare.html';
        case 'CoFacilitator':
          return 'groupParticipant/coFacilitator.html';  
        case 'Email':
          return 'profile/email.html';
        case 'Ethnicity':
          return 'profile/ethnicity.html';
        case 'FacilitatorTraining':
          return 'groupParticipant/facilitatorTraining.html';  
        case 'Gender':
          return 'profile/gender.html';
        case 'KickOffEvent':
          return 'groupParticipant/kickOffEvent.html';  
        case 'Name':
          return 'profile/name.html';
        case 'Groups': //this is used to get the sessions and will need to be refactored
          return 'groupParticipant/preferredSession.html';  
        default:
          return 'default/defaultField.html';
      }
    }
  }

})();