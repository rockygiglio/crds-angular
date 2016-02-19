(function(){
  'use strict';

  module.exports = QuestionCtrl;

  var constants = require('./constants');

  QuestionCtrl.$inject = ['$scope', '$compile', 'ImageService', 'Person', 'GROUP_TYPES'];

  function QuestionCtrl($scope, $compile, ImageService, Person, GROUP_TYPES) {

    $scope.states = constants.US_STATES;
    $scope.header = $compile('<span>' + $scope.definition.header + '<span>')($scope);
    $scope.description = $compile('<span>' + $scope.definition.description + '<span>')($scope);
    $scope.body = $compile('<span>' + $scope.definition.body + '<span>')($scope);
    $scope.help = $compile('<span>' + $scope.definition.help + '<span>')($scope);
    $scope.footer = $compile('<span>' + $scope.definition.footer + '<span>')($scope);

    Person.then(function(payload){
      $scope.person = payload;
      $scope.profileImage = ImageService.ProfileImageBaseURL + $scope.person.contactId;
      $scope.defaultImage = ImageService.DefaultProfileImage;
    });

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
      if(e.keyCode === 13 && $scope.definition.input_type !== 'textarea') {
        $scope.$parent.nextQuestion();
      }
    };

  }

})();
