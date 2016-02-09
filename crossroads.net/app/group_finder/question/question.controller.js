(function(){
  'use strict';

  module.exports = QuestionCtrl;

  var constants = require('./constants');

  QuestionCtrl.$inject = ['$scope'];

  function QuestionCtrl($scope) {

    $scope.states = constants.US_STATES;

    $scope.model = function() {
      return $scope.definition.model;
    };

    $scope.getTemplateUrl = function () {
      return 'question/input_'+ $scope.definition.input_type +'.html';
    };

    $scope.checkError = function() {
      $scope.$parent.applyErrors();
    };

  }

})();
