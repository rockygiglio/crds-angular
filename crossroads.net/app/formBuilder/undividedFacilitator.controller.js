(function(){
  'use strict';

  module.exports = UndividedFacilitatorCtrl;

  UndividedFacilitatorCtrl.$inject = ['$rootScope', 'Group', 'Session'];

  function UndividedFacilitatorCtrl($rootScope, Group, Session){
    var vm = this;

    vm.saving = false;
    vm.save = save;
    vm.groupId = 166572;
    vm.groupRole = require('crds-constants').GROUP_ROLES;
    vm.responses = {};
    
    function save() {
      vm.save = true;
      
      var participant = [{
              capacity: 1,
              contactId: parseInt(Session.exists('userId')),
              groupRoleId: vm.groupRole.LEADER,             
             // singleAttributes: vm.responses.getSingleAttributes(vm.lookup),
             // childCareNeeded: vm.responses.data.childcare,
              sendConfirmationEmail: false
              //attributeTypes: Responses.getMultiAttributes(vm.lookup, ['date_time_week', 'date_time_weekend'])
       }];
              
      //Add Person to group
      Group.Participant.save({
        groupId: vm.groupId 
      }, participant).$promise.then(function(response) {
          $rootScope.$emit('notify', $rootScope.MESSAGES.successfullRegistration);
      }, function(error) {
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);             
          vm.save = false; 
      });
    
    }
  }

})();
