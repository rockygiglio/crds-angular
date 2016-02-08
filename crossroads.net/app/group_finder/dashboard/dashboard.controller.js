(function(){
  'use strict';

  module.exports = DashboardCtrl;

  DashboardCtrl.$inject = ['$scope', '$log', '$state', 'Profile', 'Person', 'AuthService'];

  function DashboardCtrl($scope, $log, $state, Profile, Person, AuthService) {
    var vm = this;

    if (AuthService.isAuthenticated() === false) {
      $log.debug('not logged in');
      $state.go('brave.welcome');
    }

    vm.profileData = { person: Person };
  }

})();
