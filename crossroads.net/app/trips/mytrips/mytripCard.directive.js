(function() {
  'use strict()';

  module.exports = MyTripCard;

  MyTripCard.$inject = ['$log', 'TripsUrlService', '$cookies'];

  function MyTripCard($log, TripsUrlService, $cookies) {
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
      scope.shareUrl = TripsUrlService.ShareUrl(scope.trip.eventParticipantId);

      function goalMet(totalRaised, goal) {
        return (totalRaised >= goal);
      }

      function cardName(first, last) {
        return first + ' ' + last;
      }

      function showIPromise(contactId)
      {
        var loggedInContact = $cookies.get('userId');
        return Number(loggedInContact) === contactId;
      }
    }
  }
})();
