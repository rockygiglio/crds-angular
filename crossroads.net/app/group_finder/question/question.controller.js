(function(){
  'use strict';

  module.exports = QuestionCtrl;

  var constants = require('./constants');

  QuestionCtrl.$inject = ['$scope', '$compile'];

  function QuestionCtrl($scope, $compile) {

    $scope.states = constants.US_STATES;
    $scope.header = $compile('<span>' + $scope.definition.header + '<span>')($scope);
    $scope.description = $compile('<span>' + $scope.definition.description + '<span>')($scope);
    $scope.body = $compile('<span>' + $scope.definition.body + '<span>')($scope);
    $scope.footer = $compile('<span>' + $scope.definition.footer + '<span>')($scope);

    $scope.model = function() {
      return $scope.definition.model;
    };

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

  }

})();
