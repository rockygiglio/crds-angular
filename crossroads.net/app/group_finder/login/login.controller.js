(function(){
  'use strict';

  module.exports = LoginCtrl;

  LoginCtrl.$inject = ['$log', '$state', '$cookies', 'Responses', 'AuthService', 'User', 'SERIES'];

  function LoginCtrl($log, $state, $cookies, Responses, AuthService, User, SERIES) {
    if (AuthService.isAuthenticated() === true) {
      if (User.groups.length > 1) {
        $log.debug('login.controller.js - group member: redirecting');
        $state.go(SERIES.permalink + '.dashboard');
      } else {
        $log.debug('login.controller.js - registered but not in group');
        $state.go(SERIES.permalink + '.summary');
      }
    }
  }

})();
