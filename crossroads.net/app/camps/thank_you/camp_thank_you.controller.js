class CampThankYouController {
  /* @ngInject */
  constructor(CampsService, $stateParams, $rootScope, $scope) {
    this.campsService = CampsService;
    this.stateParams = $stateParams;
    this.rootScope = $rootScope;
    this.scope = $scope;

    this.viewReady = false;
    this.submitting = false;

    this.payment = CampsService.payment;

    this.rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
      debugger;
      event.preventDefault();
    });

    this.rootScope.$on('$stateChangeSuccess', (event) => {
      debugger;
      event.preventDefault();
    });

    this.rootScope.$on('$locationChangeStart', function(evt) {
      debugger;
      event.preventDefault();
    });

    this.rootScope.$on('$routeChangeStart', function (event) {
      debugger;
      event.preventDefault();
    });

    this.scope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
      debugger;
    });
  }

  $onInit() {
    this.viewReady = true;

    this.rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
      debugger;
      event.preventDefault();
    });

    this.rootScope.$on('$stateChangeSuccess', (event) => {
      debugger;
      event.preventDefault();
    });

    this.rootScope.$on('$locationChangeStart', function(evt) {
      debugger;
      event.preventDefault();
    });

    this.rootScope.$on('$routeChangeStart', function (event) {
      debugger;
      event.preventDefault();
    });

    window.addEventListener('popstate', function(event) {
      debugger;
    });
  }

  $onDestroy() {
    debugger;
  }
}

export default CampThankYouController;
