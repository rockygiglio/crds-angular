import moment from 'moment';

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

        Trip.User.get().$promise.then((res) => {
          trip.contact = res;
          console.log(trip.contact);

          const date = res.Date_of_Birth.split('T')[0];
          const birthdate = moment(date, 'YYYY-MM-DD');

          trip.userAge = moment().diff(birthdate, 'year');
        }).catch($log.error);

        trip.shareUrl = TripsUrlService.ShareUrl(trip.eventParticipantId);
      });
    }
  }
})();
