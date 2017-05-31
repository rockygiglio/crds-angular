import invoicesModule from '../../app/invoices/invoices.module';
import InvoiceConfirmationController from '../../app/invoices/invoice-confirmation.controller';

describe('Invoice Confirmation Component', () => {
  let $componentController;
  let fixture;
  let invoicesService;
  let rootScope;
  let state;
  let sce;
  let qApi;
  const invoiceId = 344;

  const bindings = {};

  beforeEach(angular.mock.module(invoicesModule));

  beforeEach(inject(function (_$rootScope_, $injector, $sce) {
    invoicesService = $injector.get('InvoicesService');
    sce = $injector.get('$sce');
    state = $injector.get('$state');
    rootScope = $injector.get('$rootScope');
    qApi = $injector.get('$q');
    state.params = {
      invoiceId: invoiceId
    };
    fixture = new InvoiceConfirmationController(invoicesService, rootScope, state, sce);
  }));


  beforeEach(() => {
    fixture.$onInit();
  });

  describe('#onInit', () => {
    beforeEach(() => {
      fixture.$onInit();
      let deferred = qApi.defer();
      deferred.resolve({});
      spyOn(invoicesService, 'getPaymentDetails').and.callFake(function() {
        return(deferred.promise);
      });
      fixture.$onInit();
      rootScope.$digest();
    });

    it('should set urls, set the view as ready', () => {
      expect(fixture.viewReady).toBeTruthy();
    });

    it('get invoice payment details', () => {
      expect(invoicesService.getPaymentDetails).toHaveBeenCalledWith(invoiceId);
    });

  });

});
