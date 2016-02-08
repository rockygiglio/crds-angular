(function(){
  'use strict';

  module.exports = GroupQuestionsService;

  GroupQuestionsService.$inject = ['$resource'];

  function GroupQuestionsService($resource) {
    // TODO Update with a production-friendly URL or endpoint.
    var url = '/app/group_finder/data/host.questions.json';
    return $resource(url, {}, { get: { method:'GET', cache: true }});
  }

})();
