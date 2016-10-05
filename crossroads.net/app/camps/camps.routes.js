export default function CampRoutes($stateProvider) {

  var attributes = require('crds-constants').ATTRIBUTE_TYPE_IDS;

  module.exports = CampRoutes;

  CampRoutes.$inject = ['$stateProvider', '$urlMatcherFactoryProvider'];

  function CampRoutes($stateProvider, $urlMatcherFactory) {

    $urlMatcherFactory.strictMode(false);

    $stateProvider
  
      .state('campsignup', {
        parent: 'noSideBar',
        url: '/camps/summercamp',
        data: {
          isProtected: true,
          meta: {
            title: 'Camp Signup',
            description: 'Select your child you want to signup for a camp'
          }
        },
        resolve: {
          $cookies: '$cookies',
          $stateParams: '$stateParams'
        }
          
    });
  }
}