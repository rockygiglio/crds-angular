/* @ngInject */
class InvoiceConfirmationController {

  constructor(InvoicesService, $rootScope, $stateParams, $sce) {
    this.invoiceId = $stateParams.invoiceId;
    this.sce = $sce;
    this.invoicesService = InvoicesService;
    this.viewReady = false;
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
