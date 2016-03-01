(function() {
  'use strict';

  var attributes = require('crds-constants').ATTRIBUTE_TYPE_IDS;

  module.exports = GoRoutes;

  GoRoutes.$inject = ['$stateProvider', '$urlMatcherFactoryProvider', '$locationProvider'];

  function GoRoutes($stateProvider, $urlMatcherFactory, $locationProvider) {

    $urlMatcherFactory.strictMode(false);

    $stateProvider
      .state('gocincinnati', {
        parent: 'noSideBar',
        url: '/gocincinnati',
        controller: 'GoCincinnatiCtrl as goCincinnati',
        templateUrl: 'signup/signup.html',
        data: {
          meta: {
            title: 'Go Cincinnati',
            description: ''
          }
        }
      });
  }

})();
