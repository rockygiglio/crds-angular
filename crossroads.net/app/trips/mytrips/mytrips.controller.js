(function() {
  'use strict';
  module.exports = MyTripsController;
  MyTripsController.$inject = ['$log', 'MyTrips', 'TripsUrlService', 'Waivers'];

  function MyTripsController($log, MyTrips, TripsUrlService, Waivers) {
    var vm = this;

    activate();

    /////////////////////////
    //// Implementations ////
    /////////////////////////
    function activate() {
      vm.myTrips = MyTrips.myTrips;
      vm.waivers = Waivers;
      _.each(vm.myTrips, function(trip) {
        trip.shareUrl = TripsUrlService.ShareUrl(trip.eventParticipantId);
      });
    }
  }
})();
