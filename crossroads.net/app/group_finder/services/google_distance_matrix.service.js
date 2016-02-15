(function () {
  'use strict()';

  module.exports = {
    init: GoogleDistanceMatrixServiceInit,
    service: GoogleDistanceMatrixService
  };

  //
  // Export an init function to be used in an angular module run() block and load the Google Map API
  //

  GoogleDistanceMatrixServiceInit.$inject = ['GoogleDistanceMatrixService'];

  function GoogleDistanceMatrixServiceInit(GoogleDistanceMatrixService) {
    GoogleDistanceMatrixService.init();
  }

  //
  // Export a Service definition wrapping the Google Distance Matrix service
  //

  GoogleDistanceMatrixService.$inject = ['$log'];

  function GoogleDistanceMatrixService($log) {
    var service = {};

    //
    // Service API
    //

    service.init = init;
    service.distanceFromAddress = distanceFromAddress;

    //
    // Service implementation
    //

    // If the Google maps JS api hasn't been loaded, load it now
    function init() {
      if (typeof google !== 'undefined' && google && google.maps) {
        return;
      }

      $log.debug('Loading Google Maps API');
      var script = document.createElement('script');
      script.type = 'text/javascript';
      script.async = true;
      script.src = 'https://maps.googleapis.com/maps/api/js?key=AIzaSyAlv_y_ltF9naBkMhQOpg_aFas6AD6Hpzg';

      var s = document.getElementsByTagName('script')[0];
      s.parentNode.insertBefore(script, s);
    }

    function handleResponse(response, status) {
      $log.debug('Goole Distance Matrix response with status:', status);
      if (status !== google.maps.DistanceMatrixStatus.OK) {
        $log.error('Google maps could not process the request and resulted in status:', status);
        return [];
      }

      // If the origin address could not be found the rows array will exist but be empty
      var result = [];

      if (response.rows.length && response.rows[0].elements) {
        result = response.rows[0].elements;

        angular.forEach(response.destinationAddresses, function (value, key) {
          result[key].destination = value;
        });
      }

      return result;
    }

    function distanceFromAddress(startingAddress, destinationAddressList) {
      if (typeof google === 'undefined' || !google || !google.maps) {
        $log.error('Google Maps API has not been loaded by init() method');
      }

      var matrixService = new google.maps.DistanceMatrixService();
      matrixService.getDistanceMatrix({
        origins: [startingAddress],
        destinations: destinationAddressList,
        travelMode: google.maps.TravelMode.DRIVING,
        unitSystem: google.maps.UnitSystem.IMPERIAL
      }, handleResponse);
    }

    // Return the service instance
    return service;
  }

})();
