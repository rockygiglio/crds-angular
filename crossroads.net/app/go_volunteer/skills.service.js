(function() {
  'use strict';

  module.exports = SkillsService;

  SkillsService.$inject = ['$resource'];

  function SkillsService($resource) {
    return $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/govolunteer/skills');
  }

})();
