/* eslint-disable import/no-extraneous-dependencies, import/no-unresolved, import/extensions */
import constants from 'crds-constants';

/* ngInject */
class InvoicesService {
  constructor($resource, $rootScope, $stateParams, $log, AttributeTypeService, $sessionStorage) {
    this.log = $log;
    this.scope = $rootScope;
    this.stateParams = $stateParams;
    this.resource = $resource;
    this.invoicesResource = $resource(`${__GATEWAY_CLIENT_ENDPOINT__}api/invoice/:invoiceId`);
    this.invoice = {};
  }

  getInvoice(invoiceId) {
    return this.invoicesResource.get({ invoiceId }, (invoice) => {
      this.invoice = invoice;
    },

    (err) => {
      this.log.error(err);
    }).$promise;
  }
}

export default InvoicesService;
