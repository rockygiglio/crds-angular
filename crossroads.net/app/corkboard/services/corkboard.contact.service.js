(function () {
  'use strict';

  module.exports = ContactAboutPost;
  var emailEndpoint = __GATEWAY_CLIENT_ENDPOINT__ + 'corkboard/api/sendemail/';

  ContactAboutPost.$inject = ['$resource', '$http'];

  function ContactAboutPost($resource, $http) {

    return {
      post: function (post) {
        return $resource(emailEndpoint,
          post,
          {
            post: { method: 'POST' }
          });
      }
    };
  };
})();
