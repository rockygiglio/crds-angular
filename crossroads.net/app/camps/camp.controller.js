/* @ngInject */
class CampController {
  constructor(CampsService) {
    this.viewReady = false;
    this.campsService = CampsService;
  }

  $onInit() {
    this.viewReady = true;
    if (this.isSummerCamp) {
      this.campsService.campTitle = 'Summer Camp';
    } else {
      this.campsService.campTitle = this.campsService.campInfo.eventTitle;
    }
  }
}
export default CampController;
