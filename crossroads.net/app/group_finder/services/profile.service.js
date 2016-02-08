(function() {
  'use strict';

  module.exports = function($resource) {
    return {
      Person: $resource(__API_ENDPOINT__ +  'api/profile/:contactId'),
    };
  };
})();
