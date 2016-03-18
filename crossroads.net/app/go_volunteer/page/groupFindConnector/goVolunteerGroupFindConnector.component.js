(function() {
  'use strict';

  module.exports = GoVolunteerGroupFindConnector;

  GoVolunteerGroupFindConnector.$inject = ['GoVolunteerService', 'GroupConnectors'];

  function GoVolunteerGroupFindConnector(GoVolunteerService, GroupConnectors) {
    return {
      restrict: 'E',
      scope: {
        onSubmit: '&' 
      },
      bindToController: true,
      controller: GoVolunteerGroupFindConnectorController,
      controllerAs: 'goGroupFindConnector',
      templateUrl: 'groupFindConnector/goVolunteerGroupFindConnector.template.html'
    };

    function GoVolunteerGroupFindConnectorController() {
      var vm = this;
      vm.activate = activate;
      vm.createGroup = createGroup;
      vm.groupConnectors = [];
      vm.loaded = loaded;
      vm.organization = GoVolunteerService.organization;
 
      vm.activate();

      /////////////////////////

      function activate() {
        if (vm.organization.openSignup) {
          GroupConnectors.OpenOrgs.query({initiativeId: 1}, function(data) {
            vm.groupConnectors = data;
          }, handleError);
        } else {
          GroupConnectors.ByOrgId.query({orgId: vm.organization.organizationId, initiativeId: 1}, function(data) {
            vm.groupConnectors = data;
          }, handleError);
        }
      }

      function createGroup() {
        vm.onSubmit({nextState: 'unique-skills'});
      }

      function handleError(err) {
        // show error page? 
        console.log(err);
      }

      function loaded() {
        return (vm.groupConnectors !== null && vm.groupConnectors.$resolved);
      }
    }
  }

})();
