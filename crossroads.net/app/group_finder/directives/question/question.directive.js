(function(){
  'use strict';

  module.exports = QuestionDirective;

  require('./question.html');
  require('./input_radio.html');
  require('./input_text.html');
  require('./input_textarea.html');
  require('./input_number.html');
  require('./input_checkbox.html');
  require('./input_select.html');
  require('./input_address.html');
  require('./input_day_time.html');

  QuestionDirective.$inject = ['$log'];

  function QuestionDirective($log) {
    return {
      restrict: 'AE',
      scope: {
        model: '@model',
        definition: '=',
        responses: '=',
      },
      templateUrl: 'question/question.html',
      controller: require('./question.controller')
    };
  }

})();
