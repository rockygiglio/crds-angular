class CampsFamilyController {
  constructor(CampsService, $log, $rootScope) {
    this.campsService = CampsService;
    this.log = $log;
    this.rootScope = $rootScope;
  }

  $onInit() {
    this.log.debug('Camps Family Controller Initialized!');
  }

  isSignedUp(member) {
    this.log.debug('is member already signed up?');
    return member.signedUpDate !== null;
  }
}

export default CampsFamilyController;
