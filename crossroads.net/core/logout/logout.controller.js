(function () {

  'use strict';
  module.exports = LogoutController;

  LogoutController.$inject = ['$rootScope', '$scope', '$log', 'AuthService', 'Session'];

  function LogoutController($rootScope, $scope, $log, AuthService, Session) {
    if (!__CRDS_ENV__) {
      $log.debug('Inside Logout-Controller');
    }

    logout();

    function logout() {
      AuthService.logout();
      Session.redirectIfNeeded();
    }
  }
})();
