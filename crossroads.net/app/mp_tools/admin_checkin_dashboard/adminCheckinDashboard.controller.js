(function() {
  'use strict';

  module.exports = AdminCheckinDashboardController;

  AdminCheckinDashboardController.$inject = ['$log', '$filter', '$modal', '$rootScope', 'DonationService', 'GiveTransferService', 'AuthService', 'CRDS_TOOLS_CONSTANTS'];

  function AdminCheckinDashboardController($log, $filter, $modal, $rootScope, DonationService, GiveTransferService, AuthService, CRDS_TOOLS_CONSTANTS) {
    var vm = this;

    activate();

    function activate() {
      vm.impersonateDonorId = GiveTransferService.impersonateDonorId;

      DonationService.queryRecurringGifts(vm.impersonateDonorId).then(function(data) {
        vm.recurring_gifts = data;
        vm.recurring_giving_view_ready = true;
        vm.recurring_giving = true;
      }, function(error) {

        vm.recurring_giving = false;
        vm.recurring_giving_view_ready = true;
        setErrorState(error);
      });
    }

    vm.allowAdminAccess = function() {
      return (AuthService.isAuthenticated() && AuthService.isAuthorized(CRDS_TOOLS_CONSTANTS.SECURITY_ROLES.FinanceTools));
    };
  }
})();
