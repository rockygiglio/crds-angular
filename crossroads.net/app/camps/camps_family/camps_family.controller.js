class CampsFamilyController {
  constructor(CampsService, $log, $rootScope) {
    this.campsService = CampsService;
    this.log = $log;
    this.rootScope = $rootScope;
  }

  $onInit() {
    this.log.debug('Camps Family Controller Initialized!');
    const ident = this.buildCMSIdentifier();
    this.cmsMessage = this.rootScope.MESSAGES[ident].content;
  }

  buildCMSIdentifier() {
    const title = _.filter(this.campsService.campTitle, letter => !/\s/.test(letter))
      .join('')
      .toLowerCase();
    return `${title}Intro`;
  }
}

export default CampsFamilyController;
