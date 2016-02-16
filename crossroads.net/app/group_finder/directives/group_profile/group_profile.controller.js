(function(){
  'use strict';

  module.exports = GroupProfileCtrl;

  GroupProfileCtrl.$inject = ['$scope', 'ImageService', 'GROUP_TYPES'];

  function GroupProfileCtrl($scope, ImageService, GROUP_TYPES) {

    console.log('GroupProfileCtrl', $scope.details);

    $scope.getProfileImage = function() {
      return ImageService.ProfileImageBaseURL + $scope.host.contactId;
    };

    $scope.getDefaultImage = function() {
      return ImageService.DefaultProfileImage;
    };

  }

})();

