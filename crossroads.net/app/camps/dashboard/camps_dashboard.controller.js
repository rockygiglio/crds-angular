class CampDashboardController {
  constructor(CampsService) {
    this.campsService = CampsService;
    this.viewReady = false;
  }

  $onInit() {
    this.data = this.campsService.dashboard;
    this.viewReady = true;
  }

  fullName(lastName, nickName) {
    return `${nickName} ${lastName}`;
  }
}

export default CampDashboardController;
