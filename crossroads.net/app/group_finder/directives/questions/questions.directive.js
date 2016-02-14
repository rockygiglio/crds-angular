(function(){
  'use strict';

  module.exports = QuestionsDirective;

  require('./questions.html');

  QuestionsDirective.$inject = ['$log'];

  function QuestionsDirective($log) {
    return {
      restrict: 'AE',
      scope: {
        mode: '@mode',
        step: '=',
        questions: '=definitions',
        responses: '=responses'
      },
      templateUrl: 'questions/questions.html',
      controller: require('./questions.controller')
    };
  }

})();
