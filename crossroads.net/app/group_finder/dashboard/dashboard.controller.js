(function(){
  'use strict';

  module.exports = DashboardCtrl;

  DashboardCtrl.$inject = ['$scope', '$log', '$state', 'Profile', 'Person', 'AuthService', 'SERIES'];

  function DashboardCtrl($scope, $log, $state, Profile, Person, AuthService, SERIES) {
    var vm = this;

    if (AuthService.isAuthenticated() === false) {
      $log.debug('not logged in');
      $state.go(SERIES.permalink + '.welcome');
    }

    vm.profileData = { person: Person };

    vm.startOver = function() {
      $state.go(SERIES.permalink + '.summary');
    };
  }

})();
