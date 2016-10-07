
export default class GeolocationService {
  constructor($http, $modal) {
    this.http = $http;
    this.modal = $modal;
  }


  open(options) {
    this.modal.open({
      template: '<geolocation></geolocation>',
      controller: 'GeolocationController',
      controllerAs: 'geolocation',
      openedClass: 'geolocation',
      size: 'lg'
    });
  }
}