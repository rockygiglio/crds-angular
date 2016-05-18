(function() {
  'use strict';

  module.exports = UndividedFacilitatorCtrl;

  UndividedFacilitatorCtrl.$inject = ['$rootScope', 'Group', 'Session', 'ProfileReferenceData',
    'Profile'];

  function UndividedFacilitatorCtrl($rootScope, Group, Session, ProfileReferenceData,
    Profile) {
    var vm = this;

    var constants = require('crds-constants');

    vm.responses = {};
    vm.saving = false;
    vm.save = save;
    vm.viewReady = false;
    
    activate();

    function activate() {
        //TODO only load profile data if profile field in CMS form builder
      ProfileReferenceData.getInstance().then(function(response) {
        debugger;
        vm.responses.genders = response.genders;
        vm.responses.maritalStatuses = response.maritalStatuses;
        vm.responses.serviceProviders = response.serviceProviders;
        //vm.states = response.states;
        //vm.countries = response.countries;
        //vm.crossroadsLocations = response.crossroadsLocations;

        Profile.Personal.get(function(data) {
          debugger;
          vm.responses.profileData = { person: data };
          
          vm.viewReady = true;
        });

      });
    }

    function save() {
      vm.saving = true;

      try {
        var singleAttributes = _.cloneDeep(vm.responses.singleAttributes);
        var coFacilitator = vm.responses[constants.CMS.FORM_BUILDER.FIELD_NAME.COFACILITATOR];

        if (coFacilitator && coFacilitator !== '') {

          var item = {
            attribute: {
              attributeId: constants.ATTRIBUTE_IDS.COFACILITATOR
            },
            notes: coFacilitator,
          };

          singleAttributes[constants.ATTRIBUTE_TYPE_IDS.COFACILITATOR] = item;
        }

        var participant = [{
          capacity: 1,
          contactId: parseInt(Session.exists('userId')),
          groupRoleId: constants.GROUP.ROLES.LEADER,
          childCareNeeded: vm.responses.Childcare,
          sendConfirmationEmail: false,
          singleAttributes: singleAttributes,
          attributeTypes: {},
        }];

        Group.Participant.save({
          groupId: constants.GROUP.GROUP_ID.UNDIVIDED_FACILITATOR,
        }, participant).$promise.then(function(response) {
          $rootScope.$emit('notify', $rootScope.MESSAGES.successfullRegistration);
          vm.saving = false;
        }, function(error) {
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
          vm.saving = false;
        });
      }
      catch (error) {
        vm.saving = false;
        throw (error);
      }
    }
  }

})();
