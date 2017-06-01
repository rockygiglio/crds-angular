/* eslint-disable import/no-extraneous-dependencies, import/no-unresolved, import/extensions */
import constants from 'crds-constants';

/* ngInject */
class InvoicesService {
  constructor($resource, $rootScope, $stateParams, $log, AttributeTypeService, $sessionStorage) {
    this.log = $log;
    this.scope = $rootScope;
    this.stateParams = $stateParams;
    this.resource = $resource;

    this.invoicesPaymentsResource = $resource(`${__GATEWAY_CLIENT_ENDPOINT__}api/v1.0.0/invoice/:invoiceId/payments`);
    this.invoiceDetailResource = $resource(`${__GATEWAY_CLIENT_ENDPOINT__}api/v1.0.0/invoice/:invoiceId/details`);

    this.invoicePayments = {};
    this.invoiceDetail = {};
  }

  getInvoicePayments(invoiceId) {
    return this.invoicesPaymentsResource.get({ invoiceId }, (invoicePayments) => {
      this.invoicePayments = invoicePayments;
    },

    (err) => {
      this.log.error(err);
    }).$promise;
  }

  getInvoiceDetail(invoiceId) {
    return this.invoiceDetailResource.get({ invoiceId }, (invoiceDetail) => {
      this.invoiceDetail = invoiceDetail;
    },

    (err) => {
      this.log.error(err);
    }).$promise;
  }
}

export default InvoicesService;
