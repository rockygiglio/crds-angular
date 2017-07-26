(function() {
  'use strict()';

  module.exports = MyTripCard;

  MyTripCard.$inject = ['$log', '$cookies', 'TripsUrlService'];

  function MyTripCard($log, $cookies, TripsUrlService) {
    return {
      restrict: 'EA',
      transclude: true,
      templateUrl: 'mytrips/mytripCard.html',
      scope: {
        trip: '=',
        waivers: '='
      },
      link: link
    };

    function link(scope, el, attr) {

      scope.cardName = cardName;
      scope.goalMet = goalMet;
      scope.shareUrl = TripsUrlService.ShareUrl(scope.trip.eventParticipantId);
      scope.showWaiver = showWaiver;
      scope.selectWaiver = selectWaiver;

      function goalMet(totalRaised, goal) {
        return (totalRaised >= goal);
      }

      function cardName(first, last) {
        return `${first} ${last}`;
      }

      function showWaiver(contactId) {
        // TODO: need to check for event_waiver before showing button
        const loggedInContact = $cookies.get('userId');
        return contactId === parseInt(loggedInContact, 10);
      }

      function selectWaiver(waiverId) {
        // TODO: navigate to waiver
      }

      function hasDocuments(waivers, iPromise) {
        return (waivers !== null && waivers.length) || iPromise;
      }
    }
  }
})();
