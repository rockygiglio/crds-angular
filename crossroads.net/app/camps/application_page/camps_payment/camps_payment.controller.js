/* @ngInject */
export default class CampPaymentController {
  constructor(CampsService, $state, $sce) {
    this.campsService = CampsService;
    this.state = $state;
    this.sce = $sce;
    this.iframeSelector = '.camp-payment-widget';
    this.viewReady = false;
  }

  $onInit() {
    this.campsService.invoiceHasPayment(this.campsService.productInfo.invoiceId)
      .then(() => {
        this.viewReady = true;
      }).catch((err) => {
        if (err.status === 302) {
          this.state.go('campsignup.family', {}, { inherit: true, location: 'replace' });
        }
      });

    // eslint-disable-next-line global-require
    this.iFrameResizer = require('iframe-resizer/js/iframeResizer.min.js');

    this.iFrames = this.iFrameResizer({
      heightCalculationMethod: 'taggedElement',
      checkOrigin: false,
      interval: -16
    }, this.iframeSelector);

    // eslint-disable-next-line no-undef
    switch (__CRDS_ENV__) {
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
    this.totalPrice = this.campsService.productInfo.basePrice + this.getOptionPrice();
    this.depositPrice = (this.campsService.productInfo.financialAssistance) ? 50 : this.campsService.productInfo.depositPrice;
  }

  $onDestroy() {
    this.closeIframes();
  }

  buildUrl() {
    const campId = this.state.toParams.campId;
    const contactId = this.state.toParams.contactId;
    const invoiceId = this.campsService.productInfo.invoiceId;
    const url = encodeURIComponent(`${this.returnUrl}/${campId}/confirmation/${contactId}`);

    return this.sce.trustAsResourceUrl(`${this.baseUrl}?type=payment&min_payment=${this.depositPrice}&invoice_id=${invoiceId}&total_cost=${this.totalPrice}&title=${this.campsService.campTitle}&url=${url}`);
  }

  closeIframes() {
    this.iFrames.forEach((frame) => {
      if (frame.iFrameResizer !== undefined) {
        frame.iFrameResizer.close();
      }
    });
  }

  getOptionPrice() {
    if (this.campsService.productInfo.options) {
      const now = moment();
      const currentOption = _.find(this.campsService.productInfo.options, (opt) => {
        const endDate = moment(opt.endDate);
        return endDate.isSame(now, 'day') || endDate.isAfter(now, 'day');
      });
      if (currentOption) {
        return currentOption.optionPrice;
      }
    }
    return 0;
  }
}
