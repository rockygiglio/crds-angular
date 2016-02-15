(function(){
  'use strict';

  module.exports = GroupFinderCtrl;

  GroupFinderCtrl.$inject = ['$state', '$rootScope', '$anchorScroll'];

  function GroupFinderCtrl($state, $rootScope, $anchorScroll) {

    // Reset scroll position to top of window whenever state changes.
    // @see https://github.com/angular-ui/ui-router/wiki#state-change-events
    $rootScope.$on('$stateChangeSuccess', function () {
      $anchorScroll();
    });

    var vm = this;
        // TODO
        vm.isHost = true;

    if(vm.isHost) {
      $state.go('group_finder.dashboard');
    } else {
      $state.go('group_finder.summary');
    }

  }

})();
