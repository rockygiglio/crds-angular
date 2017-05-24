/* @ngInject */
class InvoiceController {

  constructor(InvoicesService, $rootScope, $stateParams, $sce) {
    this.invoiceId = $stateParams.invoiceId;
    this.sce = $sce;
    this.invoicesService = InvoicesService;
    this.viewReady = false;
  }

  $onInit() {
    console.log("ON INIT");
    this.setGatewayUrls();
    console.log("THIS", this);
    console.log('abt to call getinvoice', this.invoiceId);
    this.invoicesService.getInvoice(this.invoiceId).then(
      (data) => {
        console.log("1");
        this.url = this.buildUrl(this.invoiceId, data.paymentLeft, data.paymentLeft);
      }, (err) => {
        console.log("2");
        console.error(err);
      }).finally(() => {
        console.log("3");
        this.viewReady = true;
      });
  }

  setGatewayUrls() {
    switch (__CRDS_ENV__) {
      case 'local':
        this.baseUrl = 'http://localhost:8080';
        this.returnUrl = 'http://local.crossroads.net:3000/';
        break;
      case 'int':
        this.baseUrl = 'https://embedint.crossroads.net';
        this.returnUrl = 'https://int.crossroads.net/';
        break;
      case 'demo':
        this.baseUrl = 'https://embeddemo.crossroads.net';
        this.returnUrl = 'https://demo.crossroads.net/';
        break;
      default:
        this.baseUrl = 'https://embed.crossroads.net';
        this.returnUrl = 'https://crossroads.net/';
        break;
    }
  }

  buildUrl(invoiceId, totalCost, minPayment) {
    return this.sce.trustAsResourceUrl(`${this.baseUrl}/?type=payment&invoice_id=${invoiceId}&total_cost=${totalCost}&min_payment=${minPayment}&url=${this.returnUrl}`);
  }
}
export default InvoiceController;
