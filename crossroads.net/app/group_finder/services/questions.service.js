(function() {
  'use strict';

  module.exports = function($resource) {
    return {
      Host: $resource('/app/group_finder/data/host.questions.json'),
      Participant: $resource('/app/group_finder/data/participant.questions.json'),
    };
  };
})();
