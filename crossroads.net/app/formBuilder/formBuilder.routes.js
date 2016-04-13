(function(global) {
  'use strict';
  module.exports = FormBuilderRoutes;

  FormBuilderRoutes.$inject = ['$stateProvider', '$urlMatcherFactoryProvider', '$locationProvider'];

  function FormBuilderRoutes($stateProvider, $urlMatcherFactory, $locationProvider) {
    crds_utilities.preventRouteTypeUrlEncoding($urlMatcherFactory, 'goVolunteerRouteType', /\/go-volunteer\/.*$/);
    $urlMatcherFactory.caseInsensitive(true);

    $stateProvider
      .state('form-builder', {
        parent: 'root',
        abstract: true
      });
  }
})(this);
