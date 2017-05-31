/* @ngInject */
class InvoiceConfirmationController {

  constructor(InvoicesService, $rootScope, $stateParams, $sce) {
    this.viewReady = false;
    this.invoicesService = InvoicesService;
  }

  $onInit() {
    this.invoicesService.getPaymentDetails(this.invoiceId).then(
      (data) => {
        this.paymentDetails = data;
      }, (err) => {
        console.error(err);
      }).finally(() => {
        this.viewReady = true;
      });
  }
}
export default InvoiceConfirmationController;
