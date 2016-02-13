(function(){
  'use strict';

  module.exports = JoinCtrl;

  JoinCtrl.$inject = [
                      '$scope',
                      '$log',
                      '$http',
                      '$cookies',
                      'QuestionDefinitions',
                      'Responses',
                      'SERIES'
                     ];

  function JoinCtrl($scope,
                    $log,
                    $http,
                    $cookies,
                    QuestionDefinitions,
                    Responses,
                    SERIES) {

    var vm = this;

    // Properties
    vm.questions = QuestionDefinitions.questions;
    vm.responses = Responses.data;
  }

})();
