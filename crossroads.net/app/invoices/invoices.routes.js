import redirectingTemplate from './invoice-confirmation-redirecting.html';

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
    .state('invoices.confirmation.email', {
      parent: 'noSideBar',
      url: '/invoices/:invoiceId/email',
      template: redirectingTemplate,
      resolve: {
        InvoicesService: 'InvoicesService',
        $state: '$state',
        getPaymentDetails: (InvoicesService, $state) => InvoicesService.getPaymentDetails(
            $state.toParams.invoiceId)
          .then((data) => {
            InvoicesService.sendPaymentConfirmation(
                $state.toParams.invoiceId, data.recentPaymentId)
              .finally(() => {
                const toParams = Object.assign($state.toParams, { paymentId: data.recentPaymentId });
                $state.go('invoices.confirmation', toParams, { location: 'replace' });
              })
          })
      }
    })
    .state('invoices.confirmation', {
      parent: 'noSideBar',
      url: '/invoices/:invoiceId/payments/:paymentId/confirmation',
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
