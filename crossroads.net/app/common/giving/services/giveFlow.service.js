(function() {
  'use strict';

  module.exports = GiveFlowService;

  GiveFlowService.$inject = ['Session', '$state', '$rootScope', 'GiveTransferService', '$analytics'];

  function GiveFlowService(Session, $state, $rootScope, GiveTransferService, $analytics) {

    var flowObject = {
      reset: function(object) {
        this.account = object.account;
        this.amount = object.amount;
        this.change = object.change;
        this.thankYou = object.thankYou;
        this.confirm = object.confirm;
        this.register = object.register;
        this.login = object.login;
      },

      goToAccount: function(giveForm) {
        GiveTransferService.amountSubmitted = true;
        if (giveForm.amountForm.$valid) {
          if (!GiveTransferService.view) {
            GiveTransferService.view = 'bank';
          }

          GiveTransferService.processing = true;
          if (!Session.isActive() && !GiveTransferService.processingChange) {
            this.goToLogin();
          } else {
            $analytics.pageTrack('/give/account');
            $state.go(this.account);
          }
        } else {
          $analytics.eventTrack('amountInvalid', { category: 'giveValidation' });
          GiveTransferService.processing = false;
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        }
      },

      goToLogin: function() {
        GiveTransferService.processing = true;
        Session.addRedirectRoute(this.account, '');
        $analytics.pageTrack('/give/login');
        $state.go(this.login);
      },

      goToChange: function() {
        if (!Session.isActive()) {
          this.goToLogin();
        }

        if (GiveTransferService.brand === '#library') {
          GiveTransferService.view = 'bank';
        } else {
          GiveTransferService.view = 'cc';
        }

        GiveTransferService.savedPayment = GiveTransferService.view;
        GiveTransferService.changeAccountInfo = true;
        GiveTransferService.amountSubmitted = false;
        $analytics.pageTrack('/give/change');
        $state.go(this.change);
      }
    };

    return flowObject;
  }

})();
