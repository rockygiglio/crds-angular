(function() {
  'use strict';

  module.exports = ProfilePictureViewController;

  ProfilePictureViewController.$inject = ['$rootScope', '$scope', '$timeout', 'ImageService', '$cookies'];

  function ProfilePictureViewController($rootScope, $scope, $timeout, ImageService, $cookies) {
    var vm = this;
    if (!$rootScope.userid) {
      vm.contactId = $cookies.get('userId');
    } else {
      vm.contactId = $rootScope.userid;
    }

    vm.path = ImageService.ProfileImageBaseURL + vm.contactId;
    vm.defaultImage = ImageService.DefaultProfileImage;

    $rootScope.$on('profilePhotoChanged', function(event, data) {
      $timeout(function() {
        vm.path = ImageService.ProfileImageBaseURL + $rootScope.userid + '?' + new Date().getTime();
        $rootScope.$apply();
      }, 500);
    });
  }

})();
