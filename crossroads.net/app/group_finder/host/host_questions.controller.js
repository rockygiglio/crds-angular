(function(){
  'use strict';

  module.exports = HostQuestionsCtrl;

  HostQuestionsCtrl.$inject = ['$scope', 'Responses', 'QuestionDefinitions'];

  function HostQuestionsCtrl($scope, Responses, QuestionDefinitions) {

    var vm = this;
        vm.questions = QuestionDefinitions.questions;
        vm.currentStep = $scope.$parent.currentStep;
        vm.responses = Responses.data;

    // TODO pass this to directives via attribute.
    $scope.person = $scope.$parent.person;

  }

})();
