(function() {
  'use strict';

  module.exports = ImageService;

  Image.$inject = ['$resource', '$cookies'];

  function ImageService($resource, $cookies) {
    return {
      Image: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/image/:id'),
      ProfileImage: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/image/profile/:id'),
      ProfileImageBaseURL: __GATEWAY_CLIENT_ENDPOINT__ + 'api/image/profile/',
      ImageBaseURL: __GATEWAY_CLIENT_ENDPOINT__ + 'api/image/',
      PledgeCampaignImageBaseURL: __GATEWAY_CLIENT_ENDPOINT__ + 'api/image/pledgecampaign/',
      PledgeCampaignImage: $resource(this.PledgeCampaignImageBaseURL + ':id'),
      DefaultProfileImage: '//crossroads-media.imgix.net/images/avatar.svg'
    };
  }
})();
