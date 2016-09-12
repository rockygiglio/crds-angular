import html from './formlyBuilder_playground.html';

(function() {
  'use strict';



  module.exports = FBPlaygroundRoutes;

  FBPlaygroundRoutes.$inject = ['$stateProvider', '$urlMatcherFactoryProvider', '$locationProvider'];

  function FBPlaygroundRoutes($stateProvider, $urlMatcherFactory, $locationProvider) {

    $stateProvider.state('formlybuilder-playground', {
      parent: 'noSideBar',
      url: '/formlybuilder-playground',
      template: html,
      controller: 'PlaygroundController',
      controllerAs: 'ctrl',
      data: {
        isProtected: true,
        meta: {
          title: 'FormBuilder Playground',
          description: ''
        }
      }
    });
  }

  function addTrailingSlashIfNecessary(link) {
    if (_.endsWith(link, '/') === false) {
      return link + '/';
    }

    return link;
  }

})();
