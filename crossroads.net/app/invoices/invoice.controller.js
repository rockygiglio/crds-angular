/* @ngInject */
class InvoiceController {

  constructor(InvoicesService, $rootScope, $stateParams, $sce) {
    this.campId = $stateParams.campId;
    this.sce = $sce;
    this.invoicesService = InvoicesService;
    this.viewReady = false;
  }

  $onInit() {
    console.log("oninit");
    this.invoicesService.getInvoice(3421).then(
      (data) => {
        console.log("invoice data", data);
      },
      (err) => {
        console.log("err", err);
      }).finally(
        () => {
          console.log("finnnnally");
          this.viewReady = true;
      });
  }

  buildUrl() {
    console.log("buildurl invoice", this);
    // return this.sce.trustAsResourceUrl(`http://localhost:8080/?type=donation&min_payment=123&invoice_id=1234&total_cost=1234`);
    return this.sce.trustAsResourceUrl(`http://localhost:8080/?type=payment&min_payment=123&invoice_id=1234&total_cost=1234`);
  }
}
export default InvoiceController;
