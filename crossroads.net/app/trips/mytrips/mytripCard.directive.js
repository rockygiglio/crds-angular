(function() {
  'use strict()';

  module.exports = MyTripCard;

  MyTripCard.$inject = ['$log', 'TripsUrlService', '$cookies', '$state'];

  function MyTripCard($log, TripsUrlService, $cookies, $state) {
    return {
      restrict: 'EA',
      transclude: true,
      templateUrl: 'mytrips/mytripCard.html',
      scope: {
        trip: '='
      },
      link: link
    };

    function link(scope, el, attr) {

      scope.cardName = cardName;
      scope.goalMet = goalMet;
      scope.showIPromise = showIPromise;
      scope.signIPromise = signIPromise;
      scope.shareUrl = TripsUrlService.ShareUrl(scope.trip.eventParticipantId);

      function goalMet(totalRaised, goal) {
        return (totalRaised >= goal);
      }

      function cardName(first, last) {
        return first + ' ' + last;
      }

      function showIPromise(contactId) {
        const loggedInContact = $cookies.get('userId');
        return Number(loggedInContact) === contactId;
      }

      function signIPromise(eventId) {
        console.log('sign the i promise');
        $state.go('trippromise', { eventId });
      }
    }
  }
})();
