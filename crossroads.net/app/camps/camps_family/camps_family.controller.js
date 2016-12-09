class CampsFamilyController {
  /* @ngInject */
  constructor(CampsService, $log, $rootScope, $state) {
    this.log = $log;
    this.rootScope = $rootScope;
    this.state = $state;
  }

  $onInit() {
    this.log.debug('Camps Family Controller Initialized!');
    this.cmsMessage = this.rootScope.MESSAGES[`camps_intro_${this.state.toParams.campId}`].content || this.rootScope.MESSAGES.summercampIntro.content;
  }
}

export default CampsFamilyController;
