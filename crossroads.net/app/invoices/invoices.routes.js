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
    .state('invoices.email.confirmation', {
      parent: 'noSideBar',
      url: '/invoices/:invoiceId/payments/:paymentId/email',
      template: '<invoice-confirmation></invoice-confirmation>',
      resolve: {
        InvoicesService: 'InvoicesService',
        $state: '$state',
        sendConfirmation: (InvoicesService, $state) => InvoicesService.sendPaymentConfirmation(
            $state.toParams.invoiceId,
            $state.toParams.paymentId)
          .finally(() => {
            console.log("finally....");
            const toParams = Object.assign($state.toParams, { wasPayment: true });
            $state.go('invoices.confirmation', toParams, { location: 'replace' });
          })
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
    });
}
