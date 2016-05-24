(function() {
  'use strict';
  module.exports = FormBuilderResolverService;
  FormBuilderResolverService.$inject = ['Lookup', 'Profile', '$resolve', 'FormBuilderService'];

  // Return a non-singleton object factory for getting the map of data needed
  // by the profile pages.  Controllers should use getInstance() to get a
  // a promise, which will be resolved by the Angular UI Router resolver ($resolve).
  // This is similar to the behavior implemented by the resolve property on a
  // UI Router state.
  function FormBuilderResolverService(Lookup, Profile, $resolve, FormBuilderService) {
    var attributeTypeIds = require('crds-constants').ATTRIBUTE_TYPE_IDS;

    var data = {
      availableGroups: function() {
        //TODO GroupsUndivided from   vm.field.mpField  -or- formField.field.mpField
        return FormBuilderService.Groups.query({
          templateType: 'GroupsUndivided'
        }).$promise;
      },

      // TODO: Can we make these more generic?
      undividedFacilitatorTraining: function() {
        return FormBuilderService.Attribute.get({
          attributeTypeId: attributeTypeIds.UNDIVIDED_FACILITATOR_TRAINING
        }).$promise;
      },

      // TODO: Can we make these more generic?
      undividedRsvpKickoff: function() {
        return FormBuilderService.Attribute.get({
          attributeTypeId: attributeTypeIds.UNDIVIDED_RSVP_KICKOFF
        }).$promise;
      },

      genders: function() {
        return Lookup.query({
          table: 'genders'
        }).$promise;
      },

      profile: function(contactId) {
        return Profile.Person.get({
          contactId: contactId
        }).$promise;
      },
    };

    return {
      getInstance: function(params) {
        return $resolve.resolve(data, params);
      }
    };
  }
})();
