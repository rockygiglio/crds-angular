import invoicesModule from '../../app/invoices/invoices.module';
import InvoiceController from '../../app/invoices/invoice.controller';

describe('Invoice Component', () => {
  let $componentController;
  let fixture;
  let invoicesService;
  let rootScope;
  let state;
  let sce;

  const bindings = {};

  beforeEach(angular.mock.module(invoicesModule));

  beforeEach(inject(function (_$rootScope_, $injector, $sce) {
    invoicesService = $injector.get('InvoicesService');
    sce = $injector.get('$sce');
    state = $injector.get('$state');
    rootScope = $injector.get('$rootScope');

    state.params = {
      invoiceId: 123
    };

    fixture = new InvoiceController(invoicesService, rootScope, state, sce);
  }));


  describe('Registration open', () => {
    beforeEach(() => {
      fixture.$onInit();
    });

    it('should set the view as ready', () => {
      console.log(fixture);
      spyOn(invoicesService, 'getInvoice').and.callFake(function () {
        return;
      });

      expect(fixture.baseUrl).not.toBeUndefined();
      expect(fixture.returnUrl).not.toBeUndefined();
      expect(invoicesService.getInvoice).toHaveBeenCalledWith(state.params.invoiceId);
    });

  });
});
