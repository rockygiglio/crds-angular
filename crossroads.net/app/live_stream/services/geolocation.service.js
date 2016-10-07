
export default class GeolocationService {
  constructor($http, $modal) {
    this.http = $http;
    this.modal = $modal;
  }

  open(options) {
    this.modal.open({
      template: '<button aria-label="Close" class="close" ng-click="geolocation.close()" type="button"> <span aria-hidden="true">×</span> </button><geolocation><p>(This stuff helps us figure out how many fruitcakes to make come December)</p></geolocation>',
      controller: 'GeolocationController',
      controllerAs: 'geolocation',
      openedClass: 'geolocation-modal',
      backdrop: 'static',
      size: 'lg'
    });
  }
}