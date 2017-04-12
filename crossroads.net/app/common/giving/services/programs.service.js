(function() {
  'use strict';
  module.exports = Programs;

  Programs.$inject = ['$resource'];

  function Programs($resource) {
    return {
      Programs: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/programs/:programType', {programType: '@programType'}, {
        get: { method: 'GET', isArray: true }
      }),
      AllPrograms: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/all-programs')
    };
  }

})();
