export default class GoogleMapsService {

  constructor($rootScope, $log, $q) {
    if (typeof google !== 'undefined' && google && google.maps) {
      return;
    }

    let script   = document.createElement('script');
    script.type  = 'text/javascript';
    script.async = true;
    script.src   = 'https://maps.googleapis.com/maps/api/js?key=' + __GOOGLE_API_KEY__;

    let s = document.getElementsByTagName('script')[0];
    s.parentNode.insertBefore(script, s);

    this.apiKey    = __GOOGLE_API_KEY__;
    this.rootScope = $rootScope;
    this.log       = $log;
    this.q         = $q;
  }

  reverseGeocode(lat, lng) {
    let deferred = this.q.defer();

    if (typeof google === 'undefined' || !google || !google.maps) {
      let err = 'Google Maps API has not been loaded by init() method';
      this.log.error(err);
      deferred.reject(err);
    }

    let geocoder = new google.maps.Geocoder;

    geocoder.geocode({
      location: {
        lat: lat,
        lng: lng
      }
    }, (response, status) => {
      // this.log.log(response)
      deferred.resolve(response);
    });

    return deferred.promise;
  }

  retrieveZipcode(lat, lng) {
    let deferred = this.q.defer();

    if (typeof google === 'undefined' || !google || !google.maps) {
      let err = 'Google Maps API has not been loaded by init() method';
      this.log.error(err);
      deferred.reject(err);
    }

    let geocoder = new google.maps.Geocoder;

    geocoder.geocode({
      location: {
        lat: lat,
        lng: lng
      }
    }, (response, status) => {
      // this.log.log(response)
      if (response.length) {
        let zipcodes = [];
        _.each(response, (location) => {
          _.each(location.address_components, (address) => {
            if (_.findIndex(address.types, (t) => { return t === 'postal_code'}) >= 0) {
              zipcodes.push(address.long_name);
            } 
          })
        })

        // find the duplicate values from the zipcode array as its the most accurate
        let result = _.first(_.transform(_.countBy(zipcodes), (result, count, value) => {
                      if (count > 1) result.push(value);
                     }, []));

        deferred.resolve(result);
      } else {
        deferred.reject('No Results')
      }
    });

    return deferred.promise;


  }

}