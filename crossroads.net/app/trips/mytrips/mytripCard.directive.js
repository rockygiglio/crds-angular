function MyTripCard($log, TripsUrlService, $cookies, $state) {
  function link(scope) {
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

    function signWaiver(waiverId, eventParticipantId, eventName) {
      $state.go('sign-waiver', { waiverId, eventParticipantId, title: 'Trip Waiver', eventName });
    }

    function hasDocuments(waivers) {
      return waivers !== null && waivers.length;
    }

    function showIPromise(contactId) {
      const loggedInContact = $cookies.get('userId');
      return Number(loggedInContact) === contactId;
    }

    function signIPromise(eventParticipantId) {
      $state.go('trippromise', { eventParticipantId });
    }

    /* eslint-disable no-param-reassign */
    scope.cardName = cardName;
    scope.goalMet = goalMet;
    scope.showIPromise = showIPromise;
    scope.signIPromise = signIPromise;
    scope.shareUrl = TripsUrlService.ShareUrl(scope.trip.eventParticipantId);
    scope.showWaiver = showWaiver;
    scope.signWaiver = signWaiver;
    scope.hasDocuments = hasDocuments;
    /* eslint-enable */
  }

  return {
    restrict: 'EA',
    transclude: true,
    templateUrl: 'mytrips/mytripCard.html',
    scope: {
      trip: '=',
      waivers: '='
    },
    link
  };
}
MyTripCard.$inject = ['$log', 'TripsUrlService', '$cookies', '$state'];
module.exports = MyTripCard;
