(function(){
  'use strict';

  module.exports = HostCtrl;

  HostCtrl.$inject = [
                      '$scope',
                      '$log',
                      '$http',
                      '$cookies',
                      'QuestionDefinitions',
                      'Responses',
                      'SERIES'
                     ];

  function HostCtrl($scope,
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
