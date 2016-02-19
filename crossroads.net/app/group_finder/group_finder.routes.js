(function(){
  'use strict';

  module.exports = GroupFinderRoutes;

  GroupFinderRoutes.$inject = ['$stateProvider', '$urlRouterProvider', 'SERIES' ];

  function GroupFinderRoutes($stateProvider, $urlRouterProvider, SERIES) {

    $stateProvider
      .state('group_finder', {
        url: '/' + SERIES.permalink,
        parent: 'noHeaderOrFooter',
        controller: 'GroupFinderCtrl as base',
        templateUrl: 'common/layout.html',
        resolve: {},
        data: {
          meta: {
            title: SERIES.title,
            description: ''
          }
        }
      })

      .state('group_finder.summary', {
        controller: 'SummaryCtrl as summary',
        url: '/summary',
        templateUrl: 'summary/summary.html',
        resolve: {
          Profile: 'Profile',
          Person: 'Person'
        },
        data: {
          isProtected: true,
          meta: {
            title: SERIES.title,
            description: ''
          }
        }
      })

      .state('group_finder.invitation', {
        controller: 'GroupInvitationCtrl as invitation',
        url: '/group/join/:groupId',
        templateUrl: 'invitation/invitation.html',
        resolve: {},
        data: {
          isProtected: true,
          meta: {
            title: SERIES.title,
            description: ''
          }
        }
      })

    ;

  }

})();
