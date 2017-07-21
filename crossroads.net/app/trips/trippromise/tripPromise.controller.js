export default class TripPromiseController {
  /* @ngInject() */
  constructor($state, $stateParams, Validation, Trip) {
    this.$state = $state;
    this.$stateParams = $stateParams;
    this.validation = Validation;
    this.Trip = Trip;

    this.promise = false;

    this.processing = false;
    this.tripPromiseForm = {};
  }

  submit() {
    if (this.tripPromiseForm.$valid) {
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
  }

  cancel() {
    this.$state.go('mytrips');
  }
}
