(function(){
  'use strict';

  module.exports = GroupQuestionsService;

  GroupQuestionsService.$inject = ['QuestionService'];

  function GroupQuestionsService(QuestionService) {
    // TODO Update with a production-friendly URL or endpoint.
    return QuestionService.Host;
  }

})();
