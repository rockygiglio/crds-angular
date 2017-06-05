export default function InvoicesRoutes($httpProvider, $stateProvider) {
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
        isProtected: false,
        meta: {
          title: 'Invoice',
          description: ''
        }
      }
    })
    .state('invoices.confirmation', {
      parent: 'noSideBar',
      url: '/invoices/:invoiceId/confirmation',
      template: '<invoice-confirmation></invoice-confirmation>',
      data: {
        isProtected: false,
        meta: {
          title: 'Invoice Paid',
          description: ''
        }
      }
    })
    ;
}
