(function() {
  'use strict()';
  
  module.exports = function($resource) {
    return {
      Personal: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/profile'),
      Person: $resource(__GATEWAY_CLIENT_ENDPOINT__ +  'api/profile/:contactId'),
      AdminPerson: $resource(__GATEWAY_CLIENT_ENDPOINT__ +  'api/profile/:contactId/admin'),
      Account: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/account'),
      Password: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/account/password'),
      Subscriptions: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/subscriptions'),
      Household: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/profile/household/:householdId'),
      Statement: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/donor-statement'),
      Spouse: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/profile/:contactId/spouse')
    };
  };
})();
