(function() {
  'use strict';
  
  module.exports = Organizations;

  Organizations.$inject = ['GoVolunteerService'];

  function Organizations(GoVolunteerService) {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: OrganizationsController,
      controllerAs: 'organizations',
      templateUrl: 'organizations/organizations.template.html'
    };
    
    function OrganizationsController() { 
      var vm = this;
    }
  }

})();
