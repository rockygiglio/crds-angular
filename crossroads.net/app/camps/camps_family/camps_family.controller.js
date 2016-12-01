class CampsFamilyController {
  /* @ngInject */
  constructor(CampsService, $log, $rootScope) {
    this.log = $log;
    this.rootScope = $rootScope;
  }

  $onInit() {
    this.log.debug('Camps Family Controller Initialized!');
    this.cmsMessage = this.rootScope.MESSAGES.summercampIntro.content;
  }
}

export default CampsFamilyController;
