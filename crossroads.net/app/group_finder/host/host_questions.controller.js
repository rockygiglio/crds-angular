(function(){
  'use strict';

  module.exports = HostQuestionsCtrl;

  HostQuestionsCtrl.$inject = ['$scope', 'Responses', 'QuestionDefinitions'];

  function HostQuestionsCtrl($scope, Responses, QuestionDefinitions) {

    var vm = this;
        vm.questions = QuestionDefinitions.questions;
        vm.currentStep = $scope.$parent.currentStep;
        vm.responses = Responses.data;
  }

})();
