(function() {
  'use strict';
  module.exports = FormBuilderResolverService;
  FormBuilderResolverService.$inject = ['Lookup', 'Profile', '$resolve', 'FormBuilderService', '$q', 'FormBuilderFieldsService'];

  // Return a non-singleton object factory for getting the map of data needed
  // by the profile pages.  Controllers should use getInstance() to get a
  // a promise, which will be resolved by the Angular UI Router resolver ($resolve).
  // This is similar to the behavior implemented by the resolve property on a
  // UI Router state.
  function FormBuilderResolverService(Lookup, Profile, $resolve, FormBuilderService, $q, FormBuilderFieldsService) {
    var constants = require('crds-constants');
    var attributeTypeIds = constants.ATTRIBUTE_TYPE_IDS;
    var fieldsService = FormBuilderFieldsService;

    var data = {
      availableGroups: function(fields) {
        if (!fieldsService.hasGroupParticipant()) {
          return resolvedPromise();
        }

        //TODO GroupsUndivided from   vm.field.mpField  -or- formField.field.mpField
        // Can probably use lodash to find the name of the templateType

        return FormBuilderService.Groups.query({
          templateType: 'GroupsUndivided'
        }).$promise;
      },

      // TODO: Can we make these more generic?
      undividedFacilitatorTraining: function(fields) {
        if (!fieldsService.hasGroupParticipant()) {
          return resolvedPromise();
        }

        return FormBuilderService.Attribute.get({
          attributeTypeId: attributeTypeIds.UNDIVIDED_FACILITATOR_TRAINING
        }).$promise;
      },

      // TODO: Can we make these more generic?
      undividedRsvpKickoff: function(fields) {
        if (!fieldsService.hasGroupParticipant()) {
          return resolvedPromise();
        }

        return FormBuilderService.Attribute.get({
          attributeTypeId: attributeTypeIds.UNDIVIDED_RSVP_KICKOFF
        }).$promise;
      },

      genders: function(fields) {
        if (!fieldsService.hasProfile()) {
          return resolvedPromise();
        }

        return Lookup.query({
          table: 'genders'
        }).$promise;
      },

      profile: function(fields, contactId) {
        if (!fieldsService.hasProfile()) {
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

    return {
      getInstance: function(params) {
        return $resolve.resolve(data, params);
      }
    };
  }
})();
