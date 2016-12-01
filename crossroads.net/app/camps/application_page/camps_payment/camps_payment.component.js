import template from './camps_payment.html';
import controller from './camps_payment.controller';
import { getCampProductInfo } from '../../camps.resolves';

const CampsPayment = {
  bindings: {},
  template,
  controller,
  resolve: [
    getCampProductInfo
  ]
};

export default CampsPayment;
