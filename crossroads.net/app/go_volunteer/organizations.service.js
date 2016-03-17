(function() {
  'use strict';

  module.exports = OrganizationsService;

  OrganizationsService.$inject = ['$resource'];

  function OrganizationsService($resource) {
    return {
      ByName: $resource(__API_ENDPOINT__ + 'api/organization/:name')
    };
  }

})();
