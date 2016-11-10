/* @ngInject */
export default class CampPaymentController {
  constructor(CampsService, $state, $sce) {
    this.campsService = CampsService;
    this.state = $state;
    this.sce = $sce;
    this.iframeSelector = '.hasResize';
  }

  $onInit() {
    // eslint-disable-next-line global-require
    this.iFrameResizer = require('iframe-resizer/js/iframeResizer.min.js');

    this.iFrames = this.iFrameResizer({
      heightCalculationMethod: 'bodyScroll',
      log: this.debug
    }, this.iframeSelector);

    // eslint-disable-next-line no-undef
    switch (__CRDS_ENV__) {
      case 'int':
        this.baseUrl = 'https://embedint.crossroads.net/payment';
        this.returnUrl = 'https://int.crossroads.net/camps';
        break;
      case 'demo':
        this.baseUrl = 'https://embeddemo.crossroads.net/payment';
        this.returnUrl = 'https://demo.crossroads.net/camps';
        break;
      default:
        this.baseUrl = 'https://embed.crossroads.net/payment';
        this.returnUrl = 'https://crossroads.net/camps';
        break;
    }

    this.totalPrice = this.campsService.productInfo.basePrice;
    this.depositPrice = (this.campsService.productInfo.financialAssistance) ? 50 : this.campsService.productInfo.depositPrice;
  }

  $onDestroy() {
    this.closeIframes();
  }

  buildUrl() {
    const campId = this.state.toParams.campId;
    const contactId = this.state.toParams.contactId;
    const invoiceId = this.campsService.productInfo.invoiceId;
    const url = encodeURIComponent(`${this.returnUrl}/${campId}/thank-you/${contactId}`);

    return this.sce.trustAsResourceUrl(`${this.baseUrl}?type=payment&min_payment=${this.depositPrice}&invoice_id=${invoiceId}&total_cost=${this.totalPrice}&title=${this.campsService.campTitle}&url=${url}`);
  }

  closeIframes() {
    this.iFrames.forEach((frame) => {
      if (frame.iFrameResizer !== undefined) {
        frame.iFrameResizer.close();
      }
    });
  }
}
