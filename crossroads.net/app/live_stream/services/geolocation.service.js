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

    this.mapService.retrieveZipcode(location.lat, location.lng).then((result) => {
      location.zipcode = result;
      deferred.resolve(location);
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