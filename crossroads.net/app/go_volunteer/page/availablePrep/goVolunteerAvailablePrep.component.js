(function() {
  'use strict';

  module.exports = GoVolunteerAvailablePrep;

  GoVolunteerAvailablePrep.$inject = [];

  function GoVolunteerAvailablePrep() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerAvailablePrepController,
      controllerAs: 'goAvailablePrep',
      templateUrl: 'availablePrep/goVolunteerAvailablePrep.template.html'
    };

    function GoVolunteerAvailablePrepController() {
      var vm = this;

    }
  }

})();
