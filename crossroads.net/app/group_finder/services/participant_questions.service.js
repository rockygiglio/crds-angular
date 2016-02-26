(function(){
  'use strict';

  module.exports = ParticipantQuestionsService;

  ParticipantQuestionsService.$inject = ['QuestionService'];

  function ParticipantQuestionsService(QuestionService) {
    // TODO Update with a production-friendly URL or endpoint.
    return QuestionService.Participant;
  }

})();
