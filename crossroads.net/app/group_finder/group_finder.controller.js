(function(){
  'use strict';

  module.exports = GroupFinderCtrl;

  GroupFinderCtrl.$inject = ['$log', '$state', '$cookies', 'Responses', 'AuthService', '$location'];

  function GroupFinderCtrl($log, $state, $cookies, Responses, AuthService, $location) {
    $log.debug('group_finder.controller');
    if (AuthService.isAuthenticated() && ($location.path() === '/brave/' || $location.path() === '/brave')) {
      $state.go('brave.welcome');
    }
  }

})();
