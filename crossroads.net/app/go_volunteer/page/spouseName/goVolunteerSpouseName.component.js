(function() {
  'use strict';

  module.exports = GoVolunteerSpouseName;

  GoVolunteerSpouseName.$inject = [];

  function GoVolunteerSpouseName() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerSpouseNameController,
      controllerAs: 'goSpouseName',
      templateUrl: 'spouseName/goVolunteerSpouseName.template.html'
    };

    function GoVolunteerSpouseNameController() {
      var vm = this;

    }
  }

})();
