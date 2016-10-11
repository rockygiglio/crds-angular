import Geolocation from '../models/geolocation';

export default class GeolocationService {
  constructor($http, $modal, $q, $rootScope) {
    this.http  = $http;
    this.modal = $modal;
    this.q     = $q;
    this.rootScope = $rootScope;

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
    this.hasLocation = true;
    // let deferred = this.q.defer();
    let saved = localStorage.setItem('crds-geolocation', JSON.stringify(location));

    // if (saved) {
    //   deferred.resolve('done');
    // }

    // return deferred.promise;
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