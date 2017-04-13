(function() {

  'use strict';

  module.exports = LeaveYourMarkRoutes;

  LeaveYourMarkRoutes.$inject = ['$httpProvider', '$stateProvider'];

  /**
   * This holds all of common LeaveYourMark Routes
   */
  function LeaveYourMarkRoutes($httpProvider, $stateProvider) {

    $httpProvider.defaults.useXDomain = true;

    $httpProvider.defaults.headers.common['X-Use-The-Force'] = true;

    $stateProvider
      .state('leaveyourmark', {
        parent: 'noSideBar',
        url: '/leaveyourmark',
        controller: 'LeaveYourMarkController as leaveYourMarkCtrl',
        templateUrl: 'leaveyourmark/leaveyourmark.html' //,
        // resolve: {
          
        // }
      })
  }

})();
