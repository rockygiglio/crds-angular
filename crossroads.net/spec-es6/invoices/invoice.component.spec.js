// import invoicesModule from '../../app/invoices/invoices.module';
// import InvoiceController from '../../app/invoices/invoice.controller';
//
// describe('Invoice Component', () => {
//   let $componentController;
//   let fixture;
//   let invoicesService;
//   let rootScope;
//   let state;
//   let sce;
//
//   const bindings = {};
//
//   beforeEach(angular.mock.module(invoicesModule));
//
//   beforeEach(inject(function (_$rootScope_, $injector, $sce) {
//     invoicesService = $injector.get('InvoicesService');
//     sce = $injector.get('$sce');
//     state = $injector.get('$state');
//     rootScope = $injector.get('$rootScope');
//     state.params = {
//       invoiceId: 123
//     };
//     fixture = new InvoiceController(invoicesService, rootScope, state, sce);
//   }));
//
//
//   describe('Registration open', () => {
//     beforeEach(() => {
//       fixture.$onInit();
//     });
//
//     it('should set urls, get invoice, set the view as ready', () => {
//       expect(fixture.baseUrl).not.toBeUndefined();
//       expect(fixture.returnUrl).not.toBeUndefined();
//     });
//
//   });
// });
