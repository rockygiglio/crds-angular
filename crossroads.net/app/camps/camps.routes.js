export default function CampRoutes($stateProvider, $urlMatcherFactory) {

  $urlMatcherFactory.strictMode(false);

  $stateProvider
    .state('summercamp', {
      parent: 'noSideBar',
      url: '/camps/summercamp',
      template:'<summer-camp></summer-camp>',
      data: {
        isProtected: true,
        meta: {
          title: 'Camp Signup',
          description: 'Select your child you want to signup for a camp'
        }
      },
      resolve: {
        loggedin: crds_utilities.checkLoggedin,
        $cookies: '$cookies',
        $stateParams: '$stateParams'
      }
        
  });
}   