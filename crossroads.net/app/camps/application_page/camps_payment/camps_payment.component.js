import template from './camps_payment.html';
import controller from './camps_payment.controller';

const CampsPayment = {
  bindings: {},
  template,
  controller,
  resolve: [
    // when thankyou page changes are merged, add resolve for productInfo
  ]
};

export default CampsPayment;
