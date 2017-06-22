(function() {
  'use strict()';

  module.exports = function SkillsService($resource) {
    return $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/contact/attribute/:contactId');
  };
})();
