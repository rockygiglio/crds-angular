(function() {
  'use strict';

  module.exports = function($resource) {
    return {
      GroupMail: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/sendgroupemail'),
      Mail: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/sendemail')
    };
  };
})();
