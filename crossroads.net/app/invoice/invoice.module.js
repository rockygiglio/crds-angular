console.log(0);
import constants from '../constants';
console.log(1);
import InvoiceRoutes from './invoice.routes';
console.log(2);
import InvoiceComponent from './invoice.component';
console.log(3);
import InvoiceService from './invoice.service';
console.log(4);

import './invoice.html';

export default angular.module(constants.MODULES.INVOICES, [
  constants.MODULES.CORE,
  constants.MODULES.COMMON])
  .config(InvoiceRoutes)
  .component('invoice', InvoiceComponent)
  .service('InvoiceService', InvoiceService);

