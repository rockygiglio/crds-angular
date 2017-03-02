
(() => {
  function ProfilePictureViewController($rootScope, $scope, $timeout, ImageService, $cookies) {
    const vm = this;
    if (!$rootScope.userid && !vm.contactId) {
      vm.contactId = $cookies.get('userId');
    } else {
      vm.contactId = $rootScope.userid;
    }

    vm.path = ImageService.ProfileImageBaseURL + vm.contactId;
    vm.defaultImage = ImageService.DefaultProfileImage;

    if (!vm.autoUpdate || vm.autoUpdate !== '1') {
      $rootScope.$on('profilePhotoChanged', (event, contactId) => {
        if (contactId !== undefined) {
          vm.contactId = contactId;
        }
        $timeout(() => {
          vm.path = ImageService.ProfileImageBaseURL + vm.contactId + '?' + new Date().getTime();
          $rootScope.$apply();
        }, 500);
      });
    }
  }

  module.exports = ProfilePictureViewController;

  ProfilePictureViewController.$inject = ['$rootScope', '$scope', '$timeout', 'ImageService', '$cookies'];
})();
