(function() {
  'use strict';
  module.exports = FormBuilderResolverService;
  FormBuilderResolverService.$inject = ['Lookup', 'Profile', '$resolve', 'FormBuilderService', '$q'];

  // Return a non-singleton object factory for getting the map of data needed
  // by the profile pages.  Controllers should use getInstance() to get a
  // a promise, which will be resolved by the Angular UI Router resolver ($resolve).
  // This is similar to the behavior implemented by the resolve property on a
  // UI Router state.
  function FormBuilderResolverService(Lookup, Profile, $resolve, FormBuilderService, $q) {
    var constants = require('crds-constants');
    var attributeTypeIds = constants.ATTRIBUTE_TYPE_IDS;
    var groupParticipantField = constants.CMS.FORM_BUILDER.CLASS_NAME.GROUP_PARTICIPANT_FIELD;
    var profileField = constants.CMS.FORM_BUILDER.CLASS_NAME.PROFILE_FILED;

    var data = {
      availableGroups: function(fields) {

        if (!hasFieldSection(fields, groupParticipantField)) {
          return resolvedPromise();
        }

        //TODO GroupsUndivided from   vm.field.mpField  -or- formField.field.mpField
        // Can probably use lodash to find the name of the templateType

        return FormBuilderService.Groups.query({
          templateType: 'GroupsUndivided'
        }).$promise;
      },

      // TODO: Can we make these more generic?
      availableFacilitatorTraining: function(fields) {
        if (!hasFieldSection(fields, groupParticipantField)) {
          return resolvedPromise();
        }

        return FormBuilderService.Attribute.get({
          attributeTypeId: attributeTypeIds.UNDIVIDED_FACILITATOR_TRAINING
        }).$promise;
      },

      // TODO: Can we make these more generic?
      availableRsvpKickoff: function(fields) {
        if (!hasFieldSection(fields, groupParticipantField)) {
          return resolvedPromise();
        }

        return FormBuilderService.Attribute.get({
          attributeTypeId: attributeTypeIds.UNDIVIDED_RSVP_KICKOFF
        }).$promise;
      },

      genders: function(fields) {

        if (!hasFieldSection(fields, profileField)) {
          return resolvedPromise();
        }

        return Lookup.query({
          table: 'genders'
        }).$promise;
      },

      profile: function(fields, contactId) {
        if (!hasFieldSection(fields, profileField)) {
          return resolvedPromise();
        }

        return Profile.Person.get({
          contactId: contactId
        }).$promise;
      },
    };

    function resolvedPromise() {
      var deferred = $q.defer();
      deferred.resolve();
      return deferred.promise;
    }

    function hasFieldSection(fields, section) {
      return _.some(fields, function(field) {
        return field.className === section;
      });
    }

    return {
      getInstance: function(params) {
        return $resolve.resolve(data, params);
      }
    };
  }
})();
