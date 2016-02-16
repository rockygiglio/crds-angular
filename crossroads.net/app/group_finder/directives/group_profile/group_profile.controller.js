(function(){
  'use strict';

  module.exports = GroupProfileCtrl;

  GroupProfileCtrl.$inject = ['$scope', 'ImageService', 'GROUP_TYPES'];

  function GroupProfileCtrl($scope, ImageService, GROUP_TYPES) {

    var defaultGroup = {
      groupTitle: 'Jon S',
      description: 'Your description will go here...',
      type: 0,
      time: 'Fridays at 7pm',
      attributes: ['kids welcome', 'has a cat'],
      host: { contactId: 12345 }
    };

    $scope.group = angular.isDefined($scope.group) ? $scope.group : defaultGroup;

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

