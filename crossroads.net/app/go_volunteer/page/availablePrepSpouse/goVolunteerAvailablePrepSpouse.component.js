(function() {
  'use strict';

  module.exports = GoVolunteerAvailablePrepSpouse;

  GoVolunteerAvailablePrepSpouse.$inject = [];

  function GoVolunteerAvailablePrepSpouse() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerAvailablePrepSpouseController,
      controllerAs: 'goAvailablePrepSpouse',
      templateUrl: 'availablePrepSpouse/goVolunteerAvailablePrepSpouse.template.html'
    };

    function GoVolunteerAvailablePrepSpouseController() {
      var vm = this;

    }
  }

})();
