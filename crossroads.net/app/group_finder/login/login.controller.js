(function(){
  'use strict';

  module.exports = LoginCtrl;

  LoginCtrl.$inject = ['$log', '$state', '$cookies', 'Responses', 'AuthService', 'GroupInfo', 'GoogleDistanceMatrixService'];

  function LoginCtrl($log, $state, $cookies, Responses, AuthService, GroupInfo, GoogleDistanceMatrixService) {
    var vm = this;
    vm.matrix = function() {
      GoogleDistanceMatrixService.distanceFromAddress('10031 Manor Ln, Verona KY', [
        '828 Heights Blvd, Florence, KY 41042',
        '3500 Montgomery Rd, Cincinnati, OH 45207',
        'asdfasdfa,a  a a zxxxx'
      ]);
    };

    if (AuthService.isAuthenticated() === true) {
      if (GroupInfo.getHosting().length > 0 || GroupInfo.getParticipating().length > 0) {
        $log.debug('login.controller.js - group member: redirecting');
        $state.go('group_finder.dashboard');
      } else {
        $log.debug('login.controller.js - registered but not in group');
        $state.go('group_finder.summary');
      }
    }
  }

})();
