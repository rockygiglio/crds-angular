(function() {
  'use strict';
  module.exports = FormBuilderRoutes;

  FormBuilderRoutes.$inject = ['$stateProvider', '$urlMatcherFactoryProvider', '$locationProvider'];

  function FormBuilderRoutes($stateProvider, $urlMatcherFactory, $locationProvider) {
    $urlMatcherFactory.caseInsensitive(true);

    $stateProvider
      .state('form-builder', {
        parent: 'root',
        abstract: true
      });
  }
})();
