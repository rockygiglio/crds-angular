/* @ngInject */
class CampController {
  constructor(CampsService) {
    this.viewReady = false;
    this.campsService = CampsService;
  }

  $onInit() {
    this.viewReady = true;
    this.campTitle = this.campsService.campInfo.eventTitle;
  }
}
export default CampController;
