/* @ngInject */
class InvoiceController {

  constructor(InvoiceService, $rootScope) {
    this.campId = $stateParams.campId;
    this.campsService = CampsService;
    this.viewReady = false;
  }

  $onInit() {
    this.viewReady = true;
  }
}
export default InvoiceController;
