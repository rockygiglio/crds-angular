(function() {
  'use strict';

  module.exports = function($resource) {
    return {
      Host: $resource('/app/group_finder/data/host-flow.json'),
      Participant: $resource('/app/group_finder/data/participant.questions.json'),
    };
  };
})();
