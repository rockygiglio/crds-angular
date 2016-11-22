class CampThankYouController {
  /* @ngInject */
  constructor(CampsService, $stateParams, $rootScope, $scope) {
    this.campsService = CampsService;
    this.stateParams = $stateParams;
    this.rootScope = $rootScope;
    this.scope = $scope;

    this.viewReady = false;
    this.submitting = false;

    this.payment = CampsService.payment;
  }

  $onInit() {
    this.viewReady = true;
  }

}

export default CampThankYouController;
