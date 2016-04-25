(function() {
  'use strict';

  module.exports = EventRooms;

  DonationDetails.$inject = ['$log'];

  function EventRooms($log) {
    return {
      restrict: 'EA',
      replace: true,
      scope: {
        eventRooms: '=',
      },
      templateUrl: 'templates/eventRooms.html',
      link: link
    };

    function link(scope, element, attrs) {
      activate();

      /////////////////////////////////
      ////// IMPLMENTATION DETAILS ////
      /////////////////////////////////

      function activate() {
      }
  }
})();
