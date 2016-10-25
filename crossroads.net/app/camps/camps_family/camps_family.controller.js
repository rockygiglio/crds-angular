class CampsFamilyController {
  constructor(CampsService, $log, $rootScope) {
    this.campsService = CampsService;
    this.log = $log;
    this.rootScope = $rootScope;
  }

  $onInit() {
    this.log.debug('Camps Family Controller Initialized!');
  }

  // eslint-disable-next-line class-methods-use-this
  isSignedUp(member) {
    return member.signedUpDate !== null;
  }
}

export default CampsFamilyController;
