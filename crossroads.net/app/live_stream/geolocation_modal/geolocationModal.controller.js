
export default class GeolocationModalController {
  constructor($modalInstance, GeolocationService, $rootScope) {
    this.modalInstance      = $modalInstance;
    this.geolocationService = GeolocationService;

    this.rootScope = $rootScope;

    this.rootScope.$on('geolocationModalDismiss', () => {
      this.close();
    })
  }

  close() {
    this.modalInstance.close();
  }

  dismiss() {
    this.modalInstance.close();
  }
}