(function() {
  'use strict';

  module.exports = GoVolunteerGroupConnector;

  GoVolunteerGroupConnector.$inject = [];

  function GoVolunteerGroupConnector() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerGroupConnectorController,
      controllerAs: 'goGroupConnector',
      templateUrl: 'groupConnector/goVolunteerGroupConnector.template.html'
    };

    function GoVolunteerGroupConnectorController() {
      var vm = this;

    }
  }

})();
