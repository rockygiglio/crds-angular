(function(){
  'use strict';

  module.exports = GroupCardCtrl;

  GroupCardCtrl.$inject = ['$scope', 'ImageService', 'GROUP_TYPES'];

  function GroupCardCtrl($scope, ImageService, GROUP_TYPES) {

    $scope.defaultImage = ImageService.DefaultProfileImage;

    $scope.getMemberImage = function(member) {
      return ImageService.ProfileImageBaseURL + member.contactId;
    };

    $scope.getGroupType = function() {
      return GROUP_TYPES[$scope.group.type];
    };

    $scope.groupDescription = function() {
      return 'A ' + $scope.getGroupType() + ' group meeting on ' + $scope.group.time;
    };

  }

})();
