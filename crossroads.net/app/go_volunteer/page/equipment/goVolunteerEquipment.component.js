(function() {
  'use strict';

  module.exports = GoVolunteerEquipment;

  GoVolunteerEquipment.$inject = [];

  function GoVolunteerEquipment() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerEquipmentController,
      controllerAs: 'goEquipment',
      templateUrl: 'equipment/goVolunteerEquipment.template.html'
    };

    function GoVolunteerEquipmentController() {
      var vm = this;

    }
  }

})();
