(function(){
  'use strict';

  module.exports = HostQuestionsCtrl;

  HostQuestionsCtrl.$inject = ['$scope', 'Responses', 'QuestionDefinitions', 'SERIES'];

  function HostQuestionsCtrl($scope, Responses, QuestionDefinitions, SERIES) {

    var vm = this;
        vm.questions = QuestionDefinitions.questions;
        vm.currentStep = $scope.$parent.currentStep;
        vm.responses = Responses.data;

  }

})();
