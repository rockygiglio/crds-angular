(function() {
  'use strict';

  module.exports = EventFilter;

  EventFilter.$inject = ['$log', 'EventService'];

  function EventFilter($log, EventService) {
    return {
      restrict: 'EA',
      replace: true,
      scope: {
        label: '@',
        event: '=',
        events: '='
      },
      templateUrl: 'templates/eventFilter.html',
    };
  }
})();
