(function() {
  'use strict';

  var constants = require('crds-constants');

  module.exports = FormBuilderCtrl;

  FormBuilderCtrl.$inject = ['$rootScope',
    'Group',
    'Session',
    'ContentPageService',
    'FormBuilderFieldsService',
    '$log',
    '$q',
    '$anchorScroll'
  ];

  function FormBuilderCtrl($rootScope,
                           Group,
                           Session,
                           ContentPageService,
                           FormBuilderFieldsService,
                           $log,
                           $q, 
                           $anchorScroll) {
    var vm = this;

    vm.hasForm = hasForm;
    vm.availableForm = availableForm;

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
      vm.successfulSave = false;
      vm.save = save;

      vm.group = {};
      vm.group.groupId = null;

      // TODO: Consider setting vm.data = resolvedData, may need to address templates for changes
      vm.data = {};
      vm.data.onComplete = ContentPageService.page.onCompleteMessage;
      vm.data.displayLocation = displayLocation;
      vm.data.openBirthdatePicker = openBirthdatePicker;
      vm.data.profileData = {person: ContentPageService.resolvedData.profile};
      vm.data.header = ContentPageService.page.fields[0].header;
      vm.data.footer = ContentPageService.page.fields[0].footer;

      vm.data.genders = ContentPageService.resolvedData.genders;
      vm.data.locations = ContentPageService.resolvedData.locations;
      vm.data.availableGroups = ContentPageService.resolvedData.availableGroups;
      vm.data.attributeTypes = convertAttributeTypes(ContentPageService.resolvedData.attributeTypes);
      vm.data.groupParticipant = participant;
    }

    function availableForm() {
      if (FormBuilderFieldsService.hasGroupParticipant() && vm.data.availableGroups.length < 1) {
          return false;
      }
      return true;
    }

    function displayLocation(locationId) {
      return _.result(_.find(vm.data.locations, 'dp_RecordID', locationId), 'dp_RecordName');
    }

    function openBirthdatePicker($event) {
      $event.preventDefault();
      $event.stopPropagation();
      this.birthdateOpen = !this.birthdateOpen;
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

    function resolvedPromise() {
      var deferred = $q.defer();
      deferred.resolve();
      return deferred.promise;
    }

    function save() {
      vm.saving = true;
      vm.successfulSave = false;
      try {
        var promise = savePersonal();
        promise = promise.then(saveGroup);

        promise.then(function() {
            $rootScope.$emit('notify', $rootScope.MESSAGES.successfullRegistration);
            vm.saving = false;
            vm.successfulSave = true;
            $anchorScroll();            
          },

          function() {
            $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
            $log.debug('person save unsuccessful');
            vm.saving = false;
            vm.successfulSave = false;
          });
      }
      catch (error) {
        vm.saving = false;
        vm.successfulSave = false;
        throw (error);
      }
    }

    function savePersonal() {
      if (!FormBuilderFieldsService.hasProfile()) {
        return resolvedPromise();
      }

      // set oldName to existing email address to work around password change dialog issue
      vm.data.profileData.person.oldEmail = vm.data.profileData.person.emailAddress;

      return vm.data.profileData.person.$save();
    }


    function getAttributeNote(fieldName, attributeId) {
      var field = vm.data[fieldName];

      var attribute = {
        attribute: null,
        notes: null,
      };

      if (field && field !== '') {
        attribute = {
          attribute: {
            attributeId: attributeId,
          },
          notes: field,
        };
      }

      return attribute;
    }

    function saveGroup() {
      if (!FormBuilderFieldsService.hasGroupParticipant()) {
        return resolvedPromise();
      }

      var coFacilitator = getAttributeNote(
        constants.CMS.FORM_BUILDER.FIELD_NAME.COFACILITATOR,
        constants.ATTRIBUTE_IDS.COFACILITATOR
      );
      vm.data.groupParticipant.singleAttributes[constants.ATTRIBUTE_TYPE_IDS.COFACILITATOR] = coFacilitator;

      // TODO: See if we can move this logic into the templates
      var coParticipant = getAttributeNote(
        constants.CMS.FORM_BUILDER.FIELD_NAME.COPARTICIPANT,
        constants.ATTRIBUTE_IDS.COPARTICIPANT
      );
      vm.data.groupParticipant.singleAttributes[constants.ATTRIBUTE_TYPE_IDS.COPARTICIPANT] = coParticipant;

      if (vm.data[constants.CMS.FORM_BUILDER.FIELD_NAME.COFACILITATOR]) {
        vm.data.groupParticipant.groupRoleId = constants.GROUP.ROLES.LEADER;
      }

      var participants = [vm.data.groupParticipant];

      return Group.Participant.save({
          groupId: vm.data.group.groupId,
        },
        participants).$promise;
    }
  }
})();
