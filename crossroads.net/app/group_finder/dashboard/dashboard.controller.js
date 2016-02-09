(function(){
  'use strict';

  module.exports = DashboardCtrl;

  DashboardCtrl.$inject = ['$scope', '$log', '$state', 'Profile', 'Person', 'AuthService', 'User', 'SERIES'];

  function DashboardCtrl($scope, $log, $state, Profile, Person, AuthService, User, SERIES) {
    var vm = this;

    if (AuthService.isAuthenticated() === false) {
      $log.debug('not logged in');
      $state.go(SERIES.permalink + '.welcome');
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
    vm.startOver = function() {
      $state.go(SERIES.permalink + '.summary');
    };

    vm.driveTime = function() {
      // TODO maps api integration to calculate this
      return '18 minute';
    };
    vm.groupType = function() {
      // TODO need lookup of available group types. Waiting on CRDS API to return this value
      return 'co-ed';
    };
  }

})();
