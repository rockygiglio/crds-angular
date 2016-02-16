(function(){
  'use strict';

  module.exports = GroupProfileCtrl;

  GroupProfileCtrl.$inject = ['$scope', 'ImageService', 'GROUP_TYPES', 'GoogleDistanceMatrixService', 'Responses'];

  function GroupProfileCtrl($scope, ImageService, GROUP_TYPES, GoogleDistanceMatrixService, Responses) {

    $scope.host = $scope.group.host;
    $scope.getProfileImage = function() {
      return ImageService.ProfileImageBaseURL + $scope.host.contactId;
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
          var distance = Math.round((result[0].distance.value / 1609.344) * 10) / 10;
          return distance + ' miles away from you';
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

