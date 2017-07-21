export default class TripPromiseController {
  /* @ngInject() */
  constructor($state, $stateParams, Trip) {
    this.$state = $state;
    this.Trip = Trip;

    this.processing = false;
    this.tripPromiseForm = {};
  }

  submit() {
    this.processing = true;

    this.Trip.MyTripsPromise.post({
      eventId: this.$stateParams.eventId,
      eventParticipantId: this.myTripPromise.eventParticipantId,
      eventParticipantDocumentId: this.myTripPromise.eventParticipantDocumentId
    }).$promise.then(
      () => {
        this.$state.go('mytrips');
      },
      () => {
        this.processing = false;
      }
    );
  }

  cancel() {
    this.$state.go('mytrips');
  }
}
