import Geolocation from '../models/geolocation';

export default class GeolocationService {
  constructor($http, $modal) {
    this.http = $http;
    this.modal = $modal;
    this.haveLocation = this.hasLocation();
  }

  showModal() {
    return !this.haveLocation;
  }

  showBanner() {
    return this.haveLocation;
  }

  saveLocation(location) {
    localStorage.setItem('crds-geolocation', JSON.stringify(location));
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
  
  open(options) {
    if (this.showModal()) {
      this.modal.open({
        template: '<geolocation></geolocation>',
        controller: 'GeolocationController',
        controllerAs: 'geolocation',
        openedClass: 'geolocation',
        size: 'lg'
      });
    }
  }
}