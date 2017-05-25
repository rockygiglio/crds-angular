import constants from '../constants';
import InvoicesRoutes from './invoices.routes';
import InvoiceComponent from './invoice.component';
import InvoicesService from './invoices.service';

import './invoice.html';

export default angular.module(constants.MODULES.INVOICES, [
  constants.MODULES.CORE,
  constants.MODULES.COMMON])
  .config(InvoicesRoutes)
  .component('invoice', InvoiceComponent)
  .service('InvoicesService', InvoicesService)
  .name;
