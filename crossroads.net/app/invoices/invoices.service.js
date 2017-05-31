/* eslint-disable import/no-extraneous-dependencies, import/no-unresolved, import/extensions */
import constants from 'crds-constants';

/* ngInject */
class InvoicesService {
  constructor($resource, $rootScope, $stateParams, $log, AttributeTypeService, $sessionStorage) {
    this.log = $log;
    this.scope = $rootScope;
    this.stateParams = $stateParams;
    this.resource = $resource;
    this.invoicesResource = $resource(`${__GATEWAY_CLIENT_ENDPOINT__}api/v1.0.0/invoice/:invoiceId`);
    this.invoicesDetailsResource = $resource(`${__GATEWAY_CLIENT_ENDPOINT__}api/v1.0.0/invoice/:invoiceId/details`);
    this.invoice = {};
    this.invoiceDetails = {};
  }

  getInvoice(invoiceId) {
    return this.invoicesResource.get({ invoiceId }, (invoice) => {
      this.invoice = invoice;
    }, (err) => {
      this.log.error(err);
    }).$promise;
  }

  getPaymentDetails(invoiceId) {
    return this.invoicesDetailsResource.get({ invoiceId }, (invoiceDetails) => {
      this.invoiceDetails = invoiceDetails;
    }, (err) => {
      this.log.error(err);
    }).$promise;
  }
}

export default InvoicesService;
