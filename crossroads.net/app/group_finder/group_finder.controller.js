(function(){
  'use strict';

  module.exports = GroupFinderCtrl;

  GroupFinderCtrl.$inject = ['$rootScope', '$anchorScroll'];

  function GroupFinderCtrl($rootScope, $anchorScroll) {

    // Reset scroll position to top of window whenever state changes.
    // @see https://github.com/angular-ui/ui-router/wiki#state-change-events
    $rootScope.$on('$stateChangeSuccess', function () {
      $anchorScroll();
    });
  }

})();
