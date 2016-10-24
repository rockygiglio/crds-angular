/* @ngInject */
class CampController {
  constructor(CampsService, $rootScope, $stateParams) {
    this.viewReady = false;
    this.campsService = CampsService;
    this.rootScope = $rootScope;
    this.stateParams = $stateParams;
    this.campId = $stateParams.campId;
  }

  $onInit() {
    this.cmsMessage = this.rootScope.MESSAGES.summercampIntro.content;
    this.viewReady = true;
    this.campsService.campTitle = this.campsService.campInfo.eventTitle;
  }

}
export default CampController;
