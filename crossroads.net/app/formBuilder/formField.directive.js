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
        case 'Leader':
          return null;
        case 'Location':
          return 'profile/location.html';
        case 'Member':
          return null;
        case 'Name':
          return 'profile/name.html';
        case 'GroupsUndivided':
          return 'groupParticipant/preferredSession.html';
        default:
          return 'default/defaultField.html';
      }
    }
  }

})();
