(function() {
  'use strict';

  module.exports = GroupConnectorsService;

  GroupConnectorsService.$inject = ['$resource'];

  function GroupConnectorsService($resource) {
    return {
      OpenOrgs: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/group-connectors/open-orgs/:initiativeId'),
      ByOrgId: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/group-connectors/:orgId/:initiativeId')
    };
  }

})();
