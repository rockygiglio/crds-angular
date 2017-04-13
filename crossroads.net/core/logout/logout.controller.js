(function() {

  'use strict';
  module.exports = LogoutController;

  LogoutController.$inject = ['$rootScope', '$scope', '$log', 'AuthService', '$state', 'Session', '$location'];

  function LogoutController($rootScope, $scope, $log, AuthService, $state, Session, $location) {
    $log.debug('Inside Logout-Controller');

    logout();

    function logout(){    
        AuthService.logout();
        Session.redirectIfNeeded($state, $location);
    }
  }
})();
