(function() {
  'use strict';

  module.exports = FormField;

  FormField.$inject = ['$templateRequest', '$compile', 'Validation'];

  function FormField($templateRequest, $compile, Validation) {
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

        scope.attributeTypeIds = require('crds-constants').ATTRIBUTE_TYPE_IDS;

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
    
    function getTemplateUrl(field) {
      switch (field.className) {
        case 'ProfileField':
        case 'GroupParticipantField':
          return getCMSTemplateUrl(field);
        case 'EditableFormStep':
          return null;
        default:
          return 'default/defaultField.html';
      }
    }

    function getCMSTemplateUrl(field) {
      //TODO: See if we can simplify / possibly strategy pattern
      switch (field.templateType) {
        case 'Birthday':
          return 'profile/birthdate.html';
        case 'Childcare':
          return 'groupParticipant/childcare.html';
        case 'CoParticipant':
          return 'groupParticipant/coParticipant.html';
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
        case 'Location':
          return 'profile/location.html';
        case 'Name':
          return 'profile/name.html';
        case 'GroupsUndivided':
          return 'groupParticipant/preferredSession.html';
        default:
          return 'default/defaultField.html';
      }
    }
    
    function FormFieldController() {
      var vm = this;
      vm.validate = validate;
      // vm.openBirthdatePicker = openBirthdatePicker;    
  
      //  function openBirthdatePicker($event) {
      //    $event.preventDefault();
      //    $event.stopPropagation();
      //    vm.birthdateOpen = !vm.birthdateOpen;
      //  }
      
      // function validate(form, field) {
      //   return Validation.showErrors(form, field);
      // }
      
    }
  }

})();
