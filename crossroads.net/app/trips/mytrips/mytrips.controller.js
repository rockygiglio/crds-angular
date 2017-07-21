(function() {
  'use strict';
  module.exports = MyTripsController;
  MyTripsController.$inject = ['$log', 'MyTrips', 'TripsUrlService', 'Trip'];

  function MyTripsController($log, MyTrips, TripsUrlService, Trip) {
    var vm = this;

    activate();

    /////////////////////////
    //// Implementations ////
    /////////////////////////
    function activate() {
      vm.myTrips = MyTrips.myTrips;

      _.each(vm.myTrips, function(trip) {
        trip.waivers = Trip.Waivers.get({ eventId: trip.eventId }).$promise.then((res) => {
          console.log(res);
        }).catch(console.error);
        trip.shareUrl = TripsUrlService.ShareUrl(trip.eventParticipantId);
      });
    }
  }
})();
