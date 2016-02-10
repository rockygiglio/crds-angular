(function(){
  'use strict';

  module.exports = QuestionCtrl;

  var constants = require('./constants');

  QuestionCtrl.$inject = ['$scope', '$compile'];

  function QuestionCtrl($scope, $compile) {

    $scope.states = constants.US_STATES;
    $scope.tpl = $compile('<span>' + $scope.definition.question + '<span>')($scope);

    $scope.model = function() {
      return $scope.definition.model;
    };

    $scope.getTemplateUrl = function () {
      return 'question/input_'+ $scope.definition.input_type +'.html';
    };

    $scope.checkError = function() {
      $scope.$parent.applyErrors();
    };

    $scope.renderedLabel = function() {
      return $scope.tpl.html();
    };

  }

})();
