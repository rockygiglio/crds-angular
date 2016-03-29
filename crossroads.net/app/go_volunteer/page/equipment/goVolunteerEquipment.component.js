(function() {
  'use strict';

  module.exports = GoVolunteerEquipment;

  GoVolunteerEquipment.$inject = ['GoVolunteerService'];

  function GoVolunteerEquipment(GoVolunteerService) {
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
      vm.equipment = GoVolunteerService.equipment;

    }
  }

})();
