(function() {
  'use strict';
  module.exports = MyTripsController;
  MyTripsController.$inject = ['$log', 'MyTrips', 'TripsUrlService', 'Trip'];

  function MyTripsController($log, MyTrips, TripsUrlService, Trip) {
    let vm = this;

    activate();

    /***********************
     *** Implementations ***
     **********************/
    function activate() {

      vm.myTrips = MyTrips.myTrips;

      _.each(vm.myTrips, (trip) => {
        Trip.Waivers.get({ eventId: trip.eventId }).$promise.then((res) => {
          trip.waivers = res;
        }).catch($log.error);

        trip.shareUrl = TripsUrlService.ShareUrl(trip.eventParticipantId);
      });
    }
  }
})();
