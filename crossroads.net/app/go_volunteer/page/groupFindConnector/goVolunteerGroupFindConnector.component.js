(function() {
  'use strict';

  module.exports = GoVolunteerGroupFindConnector;

  GoVolunteerGroupFindConnector.$inject = ['GoVolunteerService', 'GroupConnectors'];

  function GoVolunteerGroupFindConnector(GoVolunteerService, GroupConnectors) {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerGroupFindConnectorController,
      controllerAs: 'goGroupFindConnector',
      templateUrl: 'groupFindConnector/goVolunteerGroupFindConnector.template.html'
    };

    function GoVolunteerGroupFindConnectorController() {
      var vm = this;
      vm.activate = activate;
      vm.groupConnectors = [];
      vm.organization = GoVolunteerService.organization;

      vm.activate();

      /////////////////////////

      function activate() {
        console.log('org');
        console.log(vm.organization);

        GroupConnectors.OpenOrgs.query({initiativeId: 1}, function(data) {
          vm.groupConnectors = data;
        },

        function(err) {
          console.log(err);
        });
      }
    }
  }

})();
