(function(){
  'use strict';

  module.exports = GroupFinderCtrl;

  GroupFinderCtrl.$inject = ['$state', '$rootScope', '$anchorScroll', 'AuthService'];

  function GroupFinderCtrl($state, $rootScope, $anchorScroll, AuthService) {

    // Reset scroll position to top of window whenever state changes.
    // @see https://github.com/angular-ui/ui-router/wiki#state-change-events
    $rootScope.$on('$stateChangeSuccess', function () {
      $anchorScroll();
    });

    var vm = this;
        // TODO
        // vm.isHost = true;
    if (AuthService.isAuthenticated() === false) {
      $state.go('group_finder.welcome');
    } else if ($state.$current.name === 'group_finder' || $state.$current.name === 'group_finder.welcome') {
      if(vm.isHost) {
        $state.go('group_finder.dashboard');
      } else {
        $state.go('group_finder.summary');
      }
    }

  }

})();
