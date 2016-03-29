(function() {
  'use strict';

  module.exports = GoVolunteerDataService;
  GoVolunteerDataService.$inject = ['$resource'];
  function GoVolunteerDataService($resource) {
    return {
      ProjectTypes: $resource(__API_ENDPOINT__ + 'api/goVolunteer/projectTypes'),
      PrepWork: $resource(__API_ENDPOINT__ + 'api/govolunteer/prep-times')
    };
  }
})();
