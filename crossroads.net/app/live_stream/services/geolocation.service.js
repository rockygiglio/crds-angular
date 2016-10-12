import Geolocation from '../models/geolocation';

export default class GeolocationService {
  constructor($q, $rootScope, GoogleMapsService, $analytics) {
    this.q          = $q;
    this.rootScope  = $rootScope;
    this.mapService = GoogleMapsService;
    this.analytics  = $analytics;

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

    if (typeof this.analytics !== 'undefined') {
      this.analytics.eventTrack('Streaming Location', { category: 'count', label: 'count', value: location.count })
      this.analytics.eventTrack('Streaming Location', { category: 'lat', label: 'latitude', value: location.lat })
      this.analytics.eventTrack('Streaming Location', { category: 'lng', label: 'longitude', value: location.lng })
      this.analytics.eventTrack('Streaming Location', { category: 'zipcode', label: 'zipcode', value: location.zipcode })
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