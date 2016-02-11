(function(){
  'use strict';

  module.exports = LoginCtrl;

  LoginCtrl.$inject = ['$log', '$state', '$cookies', 'Responses', 'AuthService', 'GroupInfo', 'SERIES'];

  function LoginCtrl($log, $state, $cookies, Responses, AuthService, GroupInfo, SERIES) {
    if (AuthService.isAuthenticated() === true) {
      if (GroupInfo.getHosting().length > 0 || GroupInfo.getParticipating().length > 0) {
        $log.debug('login.controller.js - group member: redirecting');
        $state.go(SERIES.permalink + '.dashboard');
      } else {
        $log.debug('login.controller.js - registered but not in group');
        $state.go(SERIES.permalink + '.summary');
      }
    }
  }

})();
