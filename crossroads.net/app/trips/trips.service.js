(function() {
  module.exports = Trip;

  Trip.$inject = ['$resource'];

  function Trip($resource) {
    return {
      Search: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/trip/search'),
      MyTrips: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/trip/mytrips'),
      MyTripsPromise: $resource(`${__GATEWAY_CLIENT_ENDPOINT__}api/v1.0.0/trip/ipromise/:eventParticipantId`),
      Email: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/donation/message'),
      TripFormResponses: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/trip/form-responses/:selectionId/:selectionCount/:recordId'),
      SaveParticipants: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/trip/participants'),
      TripParticipant: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/trip/participant/:tripParticipantId'),
      Campaign: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/trip/campaign/:campaignId'),
      WorkTeams: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/lookup?table=workteams'),
      GeneratePrivateInvites: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/trip/generate-private-invite'),
      ValidatePrivateInvite: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/trip/validate-private-invite/:pledgeCampaignId/:guid'),
      Family: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/trip/:pledgeCampaignId/family-members'),
      TripScholarship: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/trip/scholarship/:campaignId/:contactId')
    };
  }
})();
