(function(){
  'use strict';

  module.exports = GroupFinderRoutes;

  GroupFinderRoutes.$inject = ['$stateProvider', '$urlRouterProvider', 'SERIES' ];

  function GroupFinderRoutes($stateProvider, $urlRouterProvider, SERIES) {

    $stateProvider
      .state('group_finder', {
        url: '/' + SERIES.permalink,
        abstract: true,
        parent: 'noHeaderOrFooter',
        controller: 'GroupFinderCtrl as base',
        templateUrl: 'common/layout.html',
        resolve: {
          Profile: 'Profile',
          Person: 'Person',
          GroupInfo: 'GroupInfo'
        },
        data: {
          meta: {
            title: SERIES.title,
            description: ''
          }
        }
      })

      .state('group_finder.welcome', {
        controller: 'LoginCtrl as ctrl',
        url: '/welcome',
        templateUrl: 'login/welcome.html',
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
        resolve: {},
        data: {
          meta: {
            title: SERIES.title,
            description: ''
          }
        }
      })
      ;

    $urlRouterProvider
      .when('/' + SERIES.permalink, '/' + SERIES.permalink + '/welcome')
      .otherwise('/' + SERIES.permalink + '/welcome');

  }

})();
