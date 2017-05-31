import invoicesModule from '../../app/invoices/invoices.module';
import InvoiceController from '../../app/invoices/invoice.controller';

fdescribe('Invoice Component', () => {
  let $componentController;
  let fixture;
  let invoicesService;
  let rootScope;
  let stateParams;
  let sce;
  let qApi;
  const invoiceId = 344;

  beforeEach(angular.mock.module(invoicesModule));

  beforeEach(inject(function (_$rootScope_, $injector, $sce) {
    invoicesService = $injector.get('InvoicesService');
    sce = $injector.get('$sce');
    rootScope = $injector.get('$rootScope');
    stateParams = $injector.get('$stateParams');
    stateParams.invoiceId = invoiceId;
    qApi = $injector.get('$q');
    fixture = new InvoiceController(invoicesService, rootScope, stateParams, sce);
  }));

  describe('#onInit', () => {
    beforeEach(() => {
      spyOn(invoicesService, 'getInvoice').and.callFake(() => {
        const deferred = qApi.defer();
        deferred.resolve({ status: 302 });
        return deferred.promise;
      });
      fixture.$onInit();
      rootScope.$digest();
    });

    it('should set urls, set the view as ready', () => {
      expect(fixture.baseUrl).toBeDefined();
      expect(fixture.returnUrl).toBeDefined();
      expect(fixture.viewReady).toBeTruthy();
    });

    it('should set invoiceId, get invoice', () => {
      expect(fixture.invoiceId).toBe(invoiceId);
      expect(invoicesService.getInvoice).toHaveBeenCalledWith(invoiceId);
    });

  });
});
