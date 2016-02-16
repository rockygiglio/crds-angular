(function(){
  'use strict';

  module.exports = QuestionCtrl;

  var constants = require('./constants');

  QuestionCtrl.$inject = ['$scope', '$compile', 'ImageService', 'GROUP_TYPES'];

  function QuestionCtrl($scope, $compile, ImageService, GROUP_TYPES) {

    $scope.states = constants.US_STATES;
    $scope.header = $compile('<span>' + $scope.definition.header + '<span>')($scope);
    $scope.description = $compile('<span>' + $scope.definition.description + '<span>')($scope);
    $scope.body = $compile('<span>' + $scope.definition.body + '<span>')($scope);
    $scope.help = $compile('<span>' + $scope.definition.help + '<span>')($scope);
    $scope.footer = $compile('<span>' + $scope.definition.footer + '<span>')($scope);

    $scope.person = $scope.$parent.person;
    $scope.profileImage = ImageService.ProfileImageBaseURL + $scope.person.contactId;
    $scope.defaultImage = ImageService.DefaultProfileImage;
    $scope.groupType = GROUP_TYPES[$scope.responses.group_type];

    $scope.getTemplateUrl = function () {
      return 'question/input_'+ $scope.definition.input_type +'.html';
    };

    $scope.checkError = function() {
      $scope.$parent.applyErrors();
    };

    $scope.render = function(el) {
      return $scope[el].html();
    };

    $scope.onKeyUp = function(e) {
      if(e.keyCode === 13) {
        $scope.$parent.nextQuestion();
      }
    };

    $scope.getGroupType = function() {
      if(_.contains(Object.keys($scope.responses),'group_type')) {
        return GROUP_TYPES[$scope.responses.group_type];
      }
    };

    $scope.getGroupTime = function() {
      if(_.contains(Object.keys($scope.responses),'date_and_time')) {
        return $scope.responses.date_and_time['day'] + 's, ' +
               $scope.responses.date_and_time['time'] + $scope.responses.date_and_time['ampm'];
      }
    };

    // TODO Populate with actual affinities based on user responses.
    $scope.getGroupAffinities = function() {
      return [
        'Kids welcome',
        'Has a cat',
        'Has a dog'
      ];
    };

    // TODO Implement distance.
    $scope.getGroupDistance = function() {
      return '0 miles from you';
    };

  }

})();
