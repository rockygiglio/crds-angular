(function() {
  'use strict';

  module.exports = Room;

  Room.$inject = ['$resource'];

  function Room($resource) {
    return {
      ByLocation: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/room/location/:locationId'),
      ByCongregation: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/congregation/:congregationId/rooms'),
      Layouts: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/room/layouts'),
      Equipment: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/congregation/:congregationId/equipment')
    };
  }

})();
