import invoicesModule from '../../app/invoices/invoices.module';
import InvoiceConfirmationController from '../../app/invoices/invoice-confirmation.controller';

fdescribe('Invoice Confirmation Component', () => {
  let $componentController;
  let fixture;
  let invoicesService;
  let rootScope;
  let stateParams;
  let sce;
  let qApi;
  const invoiceId = 456;

  beforeEach(angular.mock.module(invoicesModule));

  beforeEach(inject(function (_$rootScope_, $injector, $sce) {
    invoicesService = $injector.get('InvoicesService');
    sce = $injector.get('$sce');
    rootScope = $injector.get('$rootScope');
    qApi = $injector.get('$q');
    stateParams = $injector.get('$stateParams');
    stateParams.invoiceId = invoiceId;
    fixture = new InvoiceConfirmationController(invoicesService, rootScope, stateParams, sce);
  }));


  beforeEach(() => {
    fixture.$onInit();
  });

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

    it('should set view as ready', () => {
      expect(fixture.viewReady).toBeTruthy();
    });

    it('set invoiceId, get invoice payment details', () => {
      expect(fixture.invoiceId).toBe(invoiceId);
      expect(invoicesService.getPaymentDetails).toHaveBeenCalledWith(invoiceId);
    });

  });

});
