(function() {
  'use strict';

  module.exports = UndividedFacilitatorCtrl;

  UndividedFacilitatorCtrl.$inject = ['$rootScope', 'Group', 'Session', 'FormBuilderService', 'ContentPageService'];

  function UndividedFacilitatorCtrl($rootScope, Group, Session, FormBuilderService, ContentPageService) {
    var vm = this;
    var constants = require('crds-constants');
    var attributeTypeIds = require('crds-constants').ATTRIBUTE_TYPE_IDS;

    //TODO Decide if you member or leader - now always leader
    var participant = {
      capacity: 1,
      contactId: parseInt(Session.exists('userId')),
      groupRoleId: constants.GROUP.ROLES.LEADER,
      childCareNeeded: false,
      sendConfirmationEmail: false,
      singleAttributes: {},
      attributeTypes: {}
    };

    vm.data = {};
    vm.saving = false;
    vm.save = save;

    // TODO: Consider setting vm.data = resolvedData, may have to add convenience methods like ethnicities

    // ProfileReferenceData
    vm.data.genders = ContentPageService.resolvedData.genders;
    vm.data.availableFacilitatorTraining = ContentPageService.resolvedData.availableFacilitatorTraining;
    vm.data.availableRsvpKickoff = ContentPageService.resolvedData.availableRsvpKickoff;

    // Person
    // TODO: Remove profileData
    vm.data.profileData = {person: ContentPageService.resolvedData.profile};
    vm.data.ethnicities = ContentPageService.resolvedData.profile.attributeTypes[attributeTypeIds.ETHNICITY].attributes;

    // FormBuilder
    //TODO make the fields generic
    vm.data.availableGroups = ContentPageService.resolvedData.availableGroups;

    vm.data.groupParticipant = participant;

    // TODO: get rid of viewReady
    vm.viewReady = true;

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
      //var singleAttributes = _.cloneDeep(vm.responses.singleAttributes);
      var coFacilitator = vm.data[constants.CMS.FORM_BUILDER.FIELD_NAME.COFACILITATOR];

      if (coFacilitator && coFacilitator !== '') {

        var item = {
            attribute: {
              attributeId: constants.ATTRIBUTE_IDS.COFACILITATOR
            },
            notes: coFacilitator,
          };
        vm.data.groupParticipant.singleAttributes[constants.ATTRIBUTE_TYPE_IDS.COFACILITATOR] = item;
      }

      var participants = [vm.data.groupParticipant];

      var group = _.find(vm.data.availableGroups, function(data) {
        return data.selected;
      });

      //TODO groupId will change with new groups
      Group.Participant.save({
          groupId: group.groupId,
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
