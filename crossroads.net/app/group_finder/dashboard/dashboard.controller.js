(function(){
  'use strict';

  module.exports = DashboardCtrl;

  DashboardCtrl.$inject = ['$scope', '$log', '$state', 'Profile', 'Person', 'AuthService', 'User'];

  function DashboardCtrl($scope, $log, $state, Profile, Person, AuthService, User) {
    var vm = this;

    if (AuthService.isAuthenticated() === false) {
      $log.debug('not logged in');
      $state.go('brave.welcome');
    }

    vm.profileData = { person: Person };
    vm.groups = User.groups;
    vm.invitee = ''; // empty invitee input text
    vm.tabs = [
      { title:'Resources', active: false, route: 'dashboard.resources' },
      { title:'My Groups', active: true, route: 'dashboard.groups'},
    ];

    vm.emailGroup = function() {
      // TODO popup with text block?
      $log.debug('Sending Email to group');
    };

    vm.inviteMember = function(email) {
      // TODO add validation, email API calls
      $log.debug('Sending Email to: ' + email);
    };
  }

})();
