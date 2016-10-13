import Geolocation from '../models/geolocation';

export default class GeolocationService {
  constructor($q, $rootScope, GoogleMapsService) {
    this.q          = $q;
    this.rootScope  = $rootScope;
    this.mapService = GoogleMapsService;

    this.answered       = false;
    this.modalDismissed = false;
  }

  showModal() {
    return !this.hasLocation();
  }

  showBanner() {
    // dismissed the modal w/o answering
    // OR you have previously answered
    return (this.modalDismissed) || (this.hasLocation() && !this.answered);
  }

  saveLocation(location) {
    this.answered    = true;
    localStorage.setItem('crds-geolocation', JSON.stringify(location));

    let formKey = '1rupjr7gvqUU203fwjmeUlIiVwCA8BdkD-mP6M6s3wxQ';
    let url = `https://docs.google.com/forms/d/${formKey}/formResponse`;
    let data = {
      "entry.1874873182": location.lat,
      "entry.1547032910": location.lng,
      "entry.1403910056": location.count,
      "entry.692424241": location.zipcode
    };
    /**
     * Using $.ajax instead of $http as $http formats data as JSON,
     * this causes google forms to not accept the data
     */
    if (typeof $ !== 'undefined') { // undefined check for tests
      $.ajax({
        url: url,
        data: data,
        type: "POST",
        dataType: "xml"
      })
    }

  }

  retrieveZipcode(location) {
    let deferred = this.q.defer();

    this.mapService.reverseGeocode(location.lat, location.lng).then((response) => {
      if (response.length) {
        let zipcodes = [],
            country  = '';
        _.each(response, (location) => {
          _.each(location.address_components, (address) => {
            if (_.findIndex(address.types, (t) => { return t === 'country'}) >= 0) {
              country = address.long_name;
            } 
            if (_.findIndex(address.types, (t) => { return t === 'postal_code'}) >= 0) {
              zipcodes.push(address.long_name);
            } 
          })
        })

        // find the duplicate values from the zipcode array as its the most accurate
        let zipcode = _.first(_.transform(_.countBy(zipcodes), (result, count, value) => {
                      if (count > 1) result.push(value);
                    }, []));

        if (country !== 'United States') {
          zipcode = 'outside US';
        }

        location.zipcode = zipcode

        deferred.resolve(location);
      } else {
        deferred.reject('No Results')
      }
      
    }, (error) => {
      deferred.reject(error);
    });

    return deferred.promise;
  }

  hasLocation() {
    return localStorage.getItem('crds-geolocation') !== null;
  }

  getLocation() {
    let locationJson = localStorage.getItem('crds-geolocation');
    let location = null;
    if (locationJson) {
      location = Geolocation.build(JSON.parse(locationJson));
    }

    return location;
  }

  success() {
    this.rootScope.$broadcast('geolocationModalDismiss');
  }

  dismissed() {
    this.modalDismissed = true;
    this.rootScope.$broadcast('geolocationModalDismiss')
  }
}