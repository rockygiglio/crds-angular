(function() {
  'use strict';

  module.exports = GoVolunteerWaiver;

  GoVolunteerWaiver.$inject = [];

  function GoVolunteerWaiver() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerWaiverController,
      controllerAs: 'goWaiver',
      templateUrl: 'waiver/goVolunteerWaiver.template.html'
    };

    function GoVolunteerWaiverController() {
      var vm = this;

    }
  }

})();
