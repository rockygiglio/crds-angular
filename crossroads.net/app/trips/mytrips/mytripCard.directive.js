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
        trip: '='
      },
      link: link
    };

    function link(scope, el, attr) {

      scope.cardName = cardName;
      scope.goalMet = goalMet;
      scope.shareUrl = TripsUrlService.ShareUrl(scope.trip.eventParticipantId);
      scope.showWaiver = showWaiver;
      scope.hasSignedWaiver = hasSignedWaiver;

      function goalMet(totalRaised, goal) {
        return (totalRaised >= goal);
      }

      function cardName(first, last) {
        return first + ' ' + last;
      }

      function showWaiver(contactId) {
        const loggedInContact = $cookies.get('userId');
        return contactId === parseInt(loggedInContact);
      }

      function hasSignedWaiver(signed) {
        return signed;
      }
    }
  }
})();
