(function() {
  'use strict';

  module.exports = UndividedFacilitatorCtrl;

  UndividedFacilitatorCtrl.$inject = ['$rootScope', 'Group', 'Session'];

  function UndividedFacilitatorCtrl($rootScope, Group, Session) {
    var vm = this;
    
    var constants = require('crds-constants');

    vm.saving = false;
    vm.save = save;
    // TODO: Make this a constant
    vm.groupId = 166572;

    vm.responses = {};

    function save() {
      vm.save = true;

      try {
        // TODO: Determine way to remove this hardcoded value here
        var coFacilitator = vm.responses[167];

        if (coFacilitator && coFacilitator !== '') {

          var item = {
            attribute: {
              attributeId: constants.ATTRIBUTE_IDS.COFACILITATOR
            },
            notes: coFacilitator,
          };

          vm.responses.singleAttributes[constants.ATTRIBUTE_TYPE_IDS.COFACILITATOR] = item;
        }

        var participant = [{
          capacity: 1,
          contactId: parseInt(Session.exists('userId')),
          groupRoleId: constants.GROUP.ROLES.LEADER,
          childCareNeeded: vm.responses.Childcare,
          sendConfirmationEmail: false,
          singleAttributes: vm.responses.singleAttributes,
          attributeTypes: {},
        }];

        //Add Person to group
        Group.Participant.save({
          groupId: vm.groupId
        }, participant).$promise.then(function (response) {
          $rootScope.$emit('notify', $rootScope.MESSAGES.successfullRegistration);
          vm.save = false;
        }, function (error) {

          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
          vm.save = false;
        });
      }
      catch (error) {
        vm.save = false;
        throw (error);
      }
      finally {
        // cleanup objects we temporarily added
        if (constants.ATTRIBUTE_TYPE_IDS.COFACILITATOR in vm.responses.singleAttributes) {
          delete vm.responses.singleAttributes[constants.ATTRIBUTE_TYPE_IDS.COFACILITATOR];
        }
      }

    }
  }

})();
