(function(){
  'use strict';

  module.exports = GroupProfileCtrl;

  GroupProfileCtrl.$inject = ['$scope', 'ImageService', 'GoogleDistanceMatrixService', 'Responses', '$state'];

  function GroupProfileCtrl($scope, ImageService, GoogleDistanceMatrixService, Responses, $state) {

    $scope.defaultGroup = {
      groupTitle: 'Chuck M.',
      type: 'Men only',
      time: 'Fridays @ 7 pm',
      imageUrl: 'https://crds-cms-uploads.imgix.net/content/images/chuck-mingo.jpg',
      attributes: ['kids welcome', 'has cats'],
      host: { contactId: 12345 },
      description: 'Hi, I\'m Chuck. I’m 37 years old, married and a dad to three adventurous kids. ' +
        'I like to run marathons, watch the Philadelphia Eagles (when they’re good) and I really like to smile. ' +
        'This is my fourth time hosting a group and I’m looking forward to connecting with some new people and getting BRAVE. ' +
        'We\'ll meet at my house in Pleasant Ridge.'
    };


    var responses = Responses.data;
    if (_.has(responses, 'completed_flow') === false) {
      $state.go('group_finder.join.questions');
    }

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

    if ($scope.group && responses.location) {
      var hostAddress = $scope.group.address.addressLine1 + ', ' +
          $scope.group.address.city + ', ' +
          $scope.group.address.state + ', ' +
          $scope.group.address.postalCode;
      var participantAddress = responses.location.street + ', ' +
          responses.location.city + ', ' +
          responses.location.state + ', ' +
          responses.location.zip;
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
      return $scope.group.type;
    };

  }

})();

