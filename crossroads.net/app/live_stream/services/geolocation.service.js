import Geolocation from '../models/geolocation';

export default class GeolocationService {
  constructor($q, $rootScope, GoogleMapsService) {
    this.q          = $q;
    this.rootScope  = $rootScope;
    this.mapService = GoogleMapsService;

    this.hasAnswered    = localStorage.getItem('crds-geolocation') !== null;
    this.answered       = false;
    this.modalDismissed = false;
  }

  showModal() {
    return !this.hasLocation();
  }

  showBanner() {
    // dismissed the modal w/o answering
    // OR you have previously answered
    return (this.modalDismissed && this.answered === false) || (this.hasAnswered && this.showModal() === false);
  }

  saveLocation(location) {
    this.answered    = true;
    localStorage.setItem('crds-geolocation', JSON.stringify(location));

    dataLayer.push({
      event:   'geolocation',
      count:   location.count,
      lat:     location.lat,
      lng:     location.lng,
      zipcode: location.zipcode
    });
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