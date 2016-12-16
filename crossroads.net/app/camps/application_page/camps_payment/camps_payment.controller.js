/* @ngInject */
export default class CampPaymentController {
  constructor(CampsService, $state, $sce) {
    this.campsService = CampsService;
    this.state = $state;
    this.sce = $sce;
    this.iframeSelector = '.camp-payment-widget';
    this.viewReady = false;
    this.update = false;
    this.redirectTo = undefined;
    this.minAmount = 10;
    this.paymentRemaining = 0;
    this.updatedTotal = CampsService.productInfo.camperInvoice.TotalAmount;
  }

  $onInit() {
    this.update = this.state.toParams.update;

    if (this.update && this.state.toParams.redirectTo) {
      this.redirectTo = this.state.toParams.redirectTo;
    }

    // eslint-disable-next-line global-require
    this.iFrameResizer = require('iframe-resizer/js/iframeResizer.min.js');

    this.iFrames = this.iFrameResizer({
      heightCalculationMethod: 'taggedElement',
      checkOrigin: false,
      interval: -16
    }, this.iframeSelector);

    // eslint-disable-next-line no-undef
    switch (__CRDS_ENV__) {
      case 'local':
        this.baseUrl = 'http://local.crossroads.net:8080';
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
    this.totalPrice = this.campsService.productInfo.basePrice + this.getOptionPrice();
    this.depositPrice = this.depositPrice();
    this.viewReady = true;
  }

  depositPrice() {
    if (this.update) {
      this.paymentRemaining = this.campsService.productInfo.camperInvoice.paymentLeft;
      this.depositPrice = this.paymentRemaining > this.minAmount ? this.minAmount : this.paymentRemaining;
    } else {
      this.depositPrice = (this.campsService.productInfo.financialAssistance) ? 50 : this.campsService.productInfo.depositPrice;
    }
    return this.depositPrice;
  }

  $onDestroy() {
    this.closeIframes();
  }

  buildUrl() {
    const campId = this.state.toParams.campId;
    const contactId = this.state.toParams.contactId;
    const invoiceId = this.campsService.productInfo.invoiceId;

    let url;
    if (this.redirectTo === 'mycamps') {
      const re = /.+(?=\/camps)/i;
      url = encodeURIComponent(`${re.exec(this.returnUrl)[0]}/${this.redirectTo}`);
    } else if (this.redirectTo) {
      url = encodeURIComponent(`${this.returnUrl}/${this.redirectTo}`);
    } else {
      url = encodeURIComponent(`${this.returnUrl}/${campId}/confirmation/${contactId}`);
    }

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
