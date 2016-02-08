(function(){
  'use strict';

  module.exports = LoginCtrl;

  LoginCtrl.$inject = ['$log', '$state', '$cookies', 'Responses', 'AuthService', 'User'];

  function LoginCtrl($log, $state, $cookies, Responses, AuthService, User) {
    if (AuthService.isAuthenticated() === true) {
      if (User.groups.length > 1) {
        $log.debug('login.controller.js - group member: redirecting');
        $state.go('brave.dashboard');
      } else {
        $log.debug('login.controller.js - registered but not in group');
        $state.go('brave.summary');
      }
    }
  }

})();
