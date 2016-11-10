/* @ngInject */
export default class CampPaymentController {
  constructor(CampsService, $state) {
    this.campsService = CampsService;
    this.state = $state;
  }

  $onInit() {
    if (this.state.toParams.invoiceId === undefined) {
      this.state.go('campsignup.application', { page: 'product-summary' });
    }
  }
}
