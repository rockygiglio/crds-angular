import InvoiceRoutes from './invoice.routes';
import constants from '../constants';

export default angular.module(constants.MODULES.INVOICES, [
  constants.MODULES.CORE,
  constants.MODULES.COMMON])
  .config(InvoiceRoutes)
  .component('crossroadsInvoice', InvoiceComponent);
