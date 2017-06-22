(function() {
  'use strict';

  module.exports = VolunteerContact;

  VolunteerContact.$inject = ['$resource'];

  function VolunteerContact($resource) {
    return {
      GroupMail: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/sendgroupemail')
    };
  }
})();
