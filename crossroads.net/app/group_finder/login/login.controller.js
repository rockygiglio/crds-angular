(function(){
  'use strict';

  module.exports = LoginCtrl;

  LoginCtrl.$inject = ['$log', '$state', '$cookies', 'Responses', 'AuthService'];

  function LoginCtrl($log, $state, $cookies, Responses, AuthService) {
    if (AuthService.isAuthenticated() === false) {
      $log.debug('not logged in');
      $state.go('brave.welcome');
    }
  }

})();
