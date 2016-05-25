(function() {
  'use strict';

  var constants = require('crds-constants');

  module.exports = FormBuilderCtrl;

  FormBuilderCtrl.$inject = ['$rootScope', 'Group', 'Session', 'ContentPageService', 'FormBuilderFieldsService'];

  function FormBuilderCtrl($rootScope, Group, Session, ContentPageService, FormBuilderFieldsService) {
    var vm = this;

    vm.hasForm = hasForm;

    activate();

    function activate() {
      if (!hasForm()) {
        return;
      }

      //TODO Decide if you member or leader - now always leader
      var participant = {
        capacity: 1,
        contactId: parseInt(Session.exists('userId')),
        groupRoleId: constants.GROUP.ROLES.MEMBER,
        childCareNeeded: false,
        sendConfirmationEmail: false,
        singleAttributes: {},
        attributeTypes: {}
      };
      participant.attributeTypes = getMultiSelectAttributeTypes(ContentPageService.resolvedData.attributeTypes);
      participant.singleAttributes = getSingleSelectAttributeTypes(ContentPageService.resolvedData.attributeTypes);

      vm.saving = false;
      vm.save = save;

      vm.group = {};
      vm.group.groupId = null;

      // TODO: Consider setting vm.data = resolvedData, may need to address templates for changes
      vm.data = {};
      vm.data.profileData = {person: ContentPageService.resolvedData.profile};
      vm.data.genders = ContentPageService.resolvedData.genders;
      vm.data.availableGroups = ContentPageService.resolvedData.availableGroups
      vm.data.attributeTypes = convertAttributeTypes(ContentPageService.resolvedData.attributeTypes);
      vm.data.groupParticipant = participant;
    }

    function convertAttributeTypes(list) {
      var results = {}
      _.each(list, function(item) {
        results[item.attributeTypeId] = item;
      });
      return results;
    }

    function getMultiSelectAttributeTypes(attributeTypes) {
      var multiAttributeTypes = _.filter(attributeTypes, function(attributeType) {
        return attributeType.allowMultipleSelections;
      });

      var results = {};
      _.each(multiAttributeTypes, function(attributeType) {
        var attributes = getMultiAttributes(attributeType);

        results[attributeType.attributeTypeId] = {
          attributeTypeId: attributeType.attributeTypeId,
          name: attributeType.name,
          attributes: attributes,
        };
      });

      return results;
    }

    function getMultiAttributes(attributeType) {
      return _.map(attributeType.attributes, function(attribute) {
        var result = {
          attributeId: attribute.attributeId,
          name: attribute.name,
          selected: false,
          startDate: null,
          endDate: null,
          notes: null,
          sortOrder: attribute.sortOrder,
          category: attribute.category,
          categoryDescription: attribute.categoryDescription
        };
        return result;
      });
    }

    function getSingleSelectAttributeTypes(attributeTypes) {
      var singleAttributeTypes = _.filter(attributeTypes, function(attributeType) {
        return !attributeType.allowMultipleSelections;
      });

      var results = {};
      _.each(singleAttributeTypes, function(attributeType) {
        results[attributeType.attributeTypeId] = {
          attribute: null,
          notes: null,
        };
      });

      return results;
    }

    function hasForm() {
      var page = ContentPageService.page;
      return (page && page.fields && page.fields.length > 1);
    }

    function hasForm() {
      return (vm.page && vm.page.fields && vm.page.fields.length > 1);
    }

    function save() {
      vm.saving = true;
      try {

        // TODO: Need to return promises from save methods and then wait on all to turn of vm.saving
        // TODO: Need to only show 1 save once all promises
        // TODO: Need to only call saves if the section is used

        savePersonal();
        saveGroup();
      }
      catch (error) {
        vm.saving = false;
        throw (error);
      }
    }

    function savePersonal() {
      if (!FormBuilderFieldsService.hasProfile()) {
        return;
      }

      // set oldName to existing email address to work around password change dialog issue
      vm.data.profileData.person.oldEmail = vm.data.profileData.person.emailAddress;

      vm.data.profileData.person.$save(function() {
           $rootScope.$emit('notify', $rootScope.MESSAGES.successfullRegistration);
           vm.saving = false;
         },

         function() {
           $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
           $log.debug('person save unsuccessful');
           vm.saving = false;
         });
    }

    function saveGroup() {
      if (!FormBuilderFieldsService.hasGroupParticipant()) {
        return;
      }

      //var singleAttributes = _.cloneDeep(vm.responses.singleAttributes);
      var coFacilitator = vm.data[constants.CMS.FORM_BUILDER.FIELD_NAME.COFACILITATOR];

      var coFacilitatorAttribute = {
        attribute: null,
        notes: null,
      };

      if (coFacilitator && coFacilitator !== '') {
        coFacilitatorAttribute = {
          attribute: {
            attributeId: constants.ATTRIBUTE_IDS.COFACILITATOR
          },
          notes: coFacilitator,
        };
      }

      vm.data.groupParticipant.singleAttributes[constants.ATTRIBUTE_TYPE_IDS.COFACILITATOR] = coFacilitatorAttribute;


      // TODO: Need better way to determine Leader vs. Member
      if (coFacilitator) {
        vm.data.groupParticipant.groupRoleId = constants.GROUP.ROLES.LEADER;
      }


      var participants = [vm.data.groupParticipant];

      Group.Participant.save({
          groupId: vm.data.group.groupId,
        }, participants).$promise.then(function(response) {
          $rootScope.$emit('notify', $rootScope.MESSAGES.successfullRegistration);
          vm.saving = false;
        }, function(error) {

          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
          vm.saving = false;
        });
    }
  }

})();
