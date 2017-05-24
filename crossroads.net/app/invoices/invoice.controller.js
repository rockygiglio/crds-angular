/* @ngInject */
class InvoiceController {

  constructor(InvoicesService, $rootScope, $stateParams, $sce) {
    this.invoiceId = $stateParams.invoiceId;
    this.sce = $sce;
    this.invoicesService = InvoicesService;
    this.viewReady = false;
  }

  $onInit() {
    this.setGatewayUrls();
    this.invoicesService.getInvoice(this.invoiceId).then(
      (data) => {
        console.log("invoice data", data);
        // TODO get cost, min payment
        this.url = this.buildUrl(this.invoiceId, 43542, 43542);
      }, (err) => {
        console.error(err);
        this.url = this.buildUrl(this.invoiceId, 43542, 43542);

      }).finally(() => {
        this.viewReady = true;
      });
  }

  setGatewayUrls() {
    console.log('__CRDS_ENV__', __CRDS_ENV__);
    switch (__CRDS_ENV__) {
      case 'local':
        this.baseUrl = 'http://localhost:8080';
        this.returnUrl = 'http://local.crossroads.net:3000/camps';
        break;
      case 'int':
        this.baseUrl = 'https://embedint.crossroads.net';
        this.returnUrl = 'https://int.crossroads.net/camps';
        break;
      case 'demo':
        this.baseUrl = 'https://embeddemo.crossroads.net';
        this.returnUrl = 'https://demo.crossroads.net/camps';
        break;
      default:
        this.baseUrl = 'https://embed.crossroads.net';
        this.returnUrl = 'https://crossroads.net/camps';
        break;
    }
  }

  buildUrl(invoiceId, totalCost, minPayment) {
    return this.sce.trustAsResourceUrl(`${this.baseUrl}/?type=payment&invoice_id=${invoiceId}&total_cost=${totalCost}&min_payment=${minPayment}&url=${this.returnUrl}`);
  }
}
export default InvoiceController;
