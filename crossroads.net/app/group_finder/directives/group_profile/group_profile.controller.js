(function(){
  'use strict';

  module.exports = GroupProfileCtrl;

  GroupProfileCtrl.$inject = ['$scope', 'ImageService', 'GROUP_TYPES'];

  function GroupProfileCtrl($scope, ImageService, GROUP_TYPES) {

    $scope.host = $scope.group.host;
    $scope.getProfileImage = function() {
      return ImageService.ProfileImageBaseURL + $scope.host.contactId;
    };

    $scope.getDefaultImage = function() {
      return ImageService.DefaultProfileImage;
    };

    $scope.getGroupDistance = function() {
      return '0 miles from you';
    };

    $scope.getGroupType = function() {
      return GROUP_TYPES[$scope.group.type];
    };

  }

})();

