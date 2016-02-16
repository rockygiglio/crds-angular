(function(){
  'use strict';

  module.exports = JoinQuestionsCtrl;

  JoinQuestionsCtrl.$inject = ['$scope', 'Responses', 'QuestionDefinitions'];

  function JoinQuestionsCtrl($scope, Responses, QuestionDefinitions) {

    var vm = this;
        vm.questions = QuestionDefinitions.questions;
        vm.currentStep = $scope.$parent.currentStep;
        vm.responses = $scope.responses = Responses.data;

  }

})();
