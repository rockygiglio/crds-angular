/* @ngInject */
class InvoiceController {

  constructor(InvoicesService, $rootScope) {
    this.campId = $stateParams.campId;
    this.campsService = CampsService;
    this.viewReady = false;
  }

  $onInit() {
    this.viewReady = true;
  }
}
export default InvoiceController;
