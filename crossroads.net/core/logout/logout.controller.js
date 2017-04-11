(function() {

  'use strict';
  module.exports = LogoutController;

  LogoutController.$inject = ['$rootScope', '$scope', '$log', 'AuthService', '$state', 'Session', 'processExternalRedirect'];

  function LogoutController($rootScope, $scope, $log, AuthService, $state, Session, processExternalRedirect) {
    $log.debug('Inside Logout-Controller');

    logout();

    function logout(){    
        AuthService.logout();
        if(processExternalRedirect !== undefined && typeof(processExternalRedirect) === 'function') {
          processExternalRedirect();
        }
        Session.redirectIfNeeded($state);
    }
  }
})();
