export default function InvoicesRoutes($stateProvider, $urlMatcherFactoryProvider) {
  $httpProvider.defaults.useXDomain = true;

  $stateProvider
    .state('invoices', {
      parent: 'noSideBar',
      abstract: true,
      data: {
        renderLegacyStyles: true
      }
    })
    .state('invoices.invoice', {
      parent: 'noSideBar',
      url: '/invoices/:invoiceId',
      template: '<invoice></invoice>',
      data: {
        isProtected: true,
        meta: {
          title: 'Invoice',
          description: ''
        }
      }
    })
    ;
}
