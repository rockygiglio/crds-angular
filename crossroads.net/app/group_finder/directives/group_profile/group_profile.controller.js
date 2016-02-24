(function(){
  'use strict';

  module.exports = GroupProfileCtrl;

  GroupProfileCtrl.$inject = ['$scope', 'ImageService', 'GROUP_TYPES', 'GoogleDistanceMatrixService', 'Responses'];

  function GroupProfileCtrl($scope, ImageService, GROUP_TYPES, GoogleDistanceMatrixService, Responses) {

    $scope.defaultGroup = {
      groupTitle: 'Brian T.',
      type: 3,
      time: 'Fridays at 7pm',
      imageUrl: 'https://s3.amazonaws.com/ample-useast/brian-tome.jpg',
      attributes: ['kids welcome', 'has cats'],
      host: { contactId: 12345 },
      description: 'Hi, I\'m Brian, a 30 something father, husband and Bengals apologist.' +
                   'This isn\'t my first rodeo, but it\'s my first time hosting a group.' +
                   'We\'ll be meeting at my humble home in Hyde Park.'
    };

    $scope.displayDefaultGroup = !angular.isDefined($scope.group);
    $scope.group = $scope.displayDefaultGroup ? $scope.defaultGroup : $scope.group;
    $scope.host = $scope.group.host;

    $scope.getProfileImage = function() {
      if($scope.displayDefaultGroup) {
        return $scope.defaultGroup.imageUrl;
      } else {
        return ImageService.ProfileImageBaseURL + $scope.host.contactId;
      }
    };

    $scope.getDefaultImage = function() {
      return ImageService.DefaultProfileImage;
    };

    if ($scope.group && Responses.data.location) {
      var hostAddress = $scope.group.addressLine1 + ', ' +
          $scope.group.city + ', ' +
          $scope.group.state + ', ' +
          $scope.group.postalCode;
      var participantAddress = Responses.data.location.street + ', ' +
          Responses.data.location.city + ', ' +
          Responses.data.location.state + ', ' +
          Responses.data.location.zip;
      GoogleDistanceMatrixService.distanceFromAddress(hostAddress, [
        participantAddress
      ]).then(function(result) {
        $scope.getGroupDistance = function() {
          if(result[0].distance) {
            var distance = Math.round((result[0].distance.value / 1609.344) * 10) / 10;
            return distance + ' miles away from you';
          }
        };
      }, function(error) {
        $scope.getGroupDistance = function() { return ''; };
      });
    }

    $scope.getGroupType = function() {
      return GROUP_TYPES[$scope.group.type];
    };

  }

})();

