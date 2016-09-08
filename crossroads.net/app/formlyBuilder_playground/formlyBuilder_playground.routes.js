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
      controllerAs: 'ctrl'
    });
  }

  function addTrailingSlashIfNecessary(link) {
    if (_.endsWith(link, '/') === false) {
      return link + '/';
    }

    return link;
  }

})();
