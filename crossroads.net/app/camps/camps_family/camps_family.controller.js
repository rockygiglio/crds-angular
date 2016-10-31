class CampsFamilyController {
  constructor(CampsService, $log, $rootScope) {
    this.campsService = CampsService;
    this.log = $log;
    this.rootScope = $rootScope;
  }

  $onInit() {
    this.log.debug('Camps Family Controller Initialized!');
    this.cmsMessage = this.rootScope.MESSAGES.summercampIntro.content;
  }

  // eslint-disable-next-line class-methods-use-this
  isSignedUp(member) {
    return member.signedUpDate !== null;
  }

  divClass(member) {
    if (!this.isSignedUp(member) && member.isEligible) {
      return 'col-sm-9 col-md-10';
    }
    return '';
  }

  pClass(member) {
    if (!this.isSignedUp(member)) {
      return 'flush-bottom';
    }

    return '';
  }
}
export default CampsFamilyController;
