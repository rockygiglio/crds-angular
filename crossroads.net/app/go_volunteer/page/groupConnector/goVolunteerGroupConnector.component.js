(function() {
  'use strict';

  module.exports = GoVolunteerGroupConnector;

  GoVolunteerGroupConnector.$inject = [];

  function GoVolunteerGroupConnector() {
    return {
      restrict: 'E',
      scope: {
        onSubmit: '&' 
      },
      bindToController: true,
      controller: GoVolunteerGroupConnectorController,
      controllerAs: 'goGroupConnector',
      templateUrl: 'groupConnector/goVolunteerGroupConnector.template.html'
    };

    function GoVolunteerGroupConnectorController() {
      var vm = this;
      vm.submit = submit;

      function submit(groupConnector) {
        if (groupConnector) {
          vm.onSubmit({nextState: 'group-find-connector'});   
        } else {
          vm.onSubmit({nextState: 'launch-site'});   
        }
      }
    }
  }

})();
