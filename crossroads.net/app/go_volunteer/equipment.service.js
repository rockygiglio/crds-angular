(function() {
  'use strict';

  module.exports = EquipmentService;

  EquipmentService.$inject = ['$resource'];

  function EquipmentService($resource) {
    return $resource(__API_ENDPOINT__ + 'api/govolunteer/equipment');
  }

})();
