(function() {
  'use strict';

  module.exports = GoVolunteerConnectorInfo;

  GoVolunteerConnectorInfo.$inject = [];

  function GoVolunteerConnectorInfo() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerConnectorInfoController,
      controllerAs: 'goVolunteerConnectorInfo',
      templateUrl: 'connectorInfo/goVolunteerConnectorInfo.template.html'
    };

    function GoVolunteerConnectorInfoController() {
      var vm = this;

    }
  }

})();
